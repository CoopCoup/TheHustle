using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IColliders
{
    public GameObject bulletPrefab;
    private Transform player;
    private Animator animator;
    private Collider2D lilCollider;
    private SpriteRenderer spriteRen;
    private Rigidbody2D rb;
    private LayerMask raycastLayerMask;

    private RoomScript room;

    //How much score this enemy is worth
    [SerializeField] private int scoreValue;


    //coroutine variables to make sure theyre null when the enemy gets killed
    private Coroutine shootCoroutine;
    private Coroutine wanderCoroutine;
    private Coroutine spawnCoroutine;

    [SerializeField] private float sightRange = 100f;

    private bool canFire = true;
    private bool canWander = true;
    private bool isDead = false;

    private Vector2 randomDestination;
    [SerializeField] private float moveSpeed = .5f;
    [SerializeField] private float wanderRadius = 10f;

    public int health = 1;

    // Define Vectors in the 8 directions 
    private Vector2 up = Vector2.up;
    private Vector2 down = Vector2.down;
    private Vector2 right = Vector2.right;
    private Vector2 left = Vector2.left;
    private Vector2 upRight = new Vector2(1, 1).normalized;
    private Vector2 downRight = new Vector2(1, -1).normalized;
    private Vector2 downLeft = new Vector2(-1, -1).normalized;
    private Vector2 upLeft = new Vector2(-1, 1).normalized;

    //make the enemy flash red when they get hit
    IEnumerator CHitEffect()
    {
        spriteRen.color = Color.red;
        yield return new WaitForSeconds(.25f);
        spriteRen.color = Color.white;
    }


    //Actually properly just die
    IEnumerator CActuallyDie()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    
    IEnumerator CShootCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(1f);
        canFire = true;
    }

    IEnumerator CPauseWander()
    {
        canWander = false;
        yield return new WaitForSeconds(.5f);
        canWander = true;
    }

    IEnumerator CSpawnCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(2f);
        canFire = true;
    }


    //Initialise the enemy in the room script, passing it a reference to the player
    public void Initialise(Transform playerRef, int difficultyValue, RoomScript roomRef)
    {
        player = playerRef;
        room = roomRef;
        //Scale the enemy's health with the game's difficulty
        if (difficultyValue >= 7)
        {
            int healthBoostChance = Random.Range(0, 3);
            if (healthBoostChance == 1)
            {
                health = 2;
            }
            else
            {
                health = 1;
            }
        }
    }
    
    //Make the enemy wander in a random direction while not firing
    private void SetRandomDestination()
    {
        //Find a random destination in a set wander radius
        randomDestination = (Vector2)transform.position + Random.insideUnitCircle * wanderRadius;

        //Check if there's a wall in the way
        RaycastHit2D hit = Physics2D.Raycast(transform.position, randomDestination - (Vector2)transform.position, wanderRadius, raycastLayerMask);
        if (hit.collider != null)
        {
            //If the destination is blocked, pick a new place to go
            randomDestination = hit.point;
        }

    }

    //Simple function that makes the enemy pick a destination and sets the wandering bool so that they move on Update
    private void StartWandering()
    {
        SetRandomDestination();
        canWander = true;
    }

    //Stop moving and shooting
    public void PauseEnemy()
    {
        if (!isDead)
        {
            if (lilCollider != null)
            {
                lilCollider.enabled = true;
            }
            canFire = false;
            canWander = false;
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
            if (wanderCoroutine != null)
            {
                StopCoroutine(wanderCoroutine);
            }
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
            }
        } 
    }

    public void ResumeEnemy()
    {
        if (!isDead)
        {
            if (lilCollider != null)
            {
                lilCollider.enabled = true;
            }
            spawnCoroutine = StartCoroutine(CSpawnCooldown());
            StartWandering();
        }
        
        
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lilCollider = GetComponent<Collider2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        //Create a layer mask so the enemies debug traces dont collide with the enemy itself
        raycastLayerMask = LayerMask.GetMask("Default", "Player");
    }

    //Implement Interface function
    public void Hit()
    {
        StartCoroutine(CHitEffect());
        if (!isDead)
        {
            health --;
            //ADD A HIT FLASH HERE --------------------------------------------------------------------------------------
            if (health <= 0)
            {
                //Play dying animation
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                }
                if (wanderCoroutine != null)
                {
                    StopCoroutine(wanderCoroutine);
                }
                if (shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                }
                Death();
            }
        }
        
    }

    //Make sure the enemy can't do anything or be hit, then animate the death
    private void Death()
    {
        isDead = true;
        canFire = false;
        canWander = false;
        animator.SetBool("IsDead", true);
        Destroy(rb);
        Destroy(lilCollider);
        room.EnemyDeath(scoreValue);
        StartCoroutine(CActuallyDie());
    }




    //If enemy bumps into player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            IColliders i = other.GetComponent<IColliders>();
            if (i != null)
            {
                i.Hit();
            }
        }
    }



    //check where the player is, if the player is in one of the lines of sight fire at the payer
    private void CheckLineOfSight()
    {
        Vector2 direction = player.position - transform.position;

        // Define the firing lines
        Vector2[] firingLines = { up, upRight, right, downRight, down, downLeft, left, upLeft };

        // Perform raycasts for each firing line
        foreach (Vector2 line in firingLines)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, line, sightRange, raycastLayerMask);
            // Check if the raycast hits the player's collider
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                if (canFire)
                {
                    Shoot(direction.normalized);
                    shootCoroutine = StartCoroutine(CShootCooldown());
                    wanderCoroutine = StartCoroutine(CPauseWander());
                    return;
                }
                
                return; // Exit the loop if player is hit
            }
        }


    }
    
    //SHOOTING fucntionality
    private void Shoot(Vector2 direction)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        EnemyBulletScript bulletRef = bulletInstance.GetComponent<EnemyBulletScript>();
        bulletRef.Initialise(direction);
    }

    // Update is called once per frame
    void Update()
    {
        CheckLineOfSight();
        if (canWander)
        {
            // Move towards the random destination
            transform.position = Vector2.MoveTowards(transform.position, randomDestination, moveSpeed * Time.deltaTime);

            // Check if the enemy has reached the destination
            if (Vector2.Distance(transform.position, randomDestination) < 2f)
            {
                // Choose a new random destination
                SetRandomDestination();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetRandomDestination();
    }
}
