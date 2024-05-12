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
    private LayerMask raycastLayerMask;
    

    [SerializeField] private float sightRange = 100f;
    private bool playerInSight = false;
    private bool canFire = true;

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

    
    IEnumerator CShootCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(1f);
        canFire = true;
    }

    IEnumerator CSpawnCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(2f);
        canFire = true;
    }


    //Initialise the enemy in the room script, passing it a reference to the player
    public void Initialise(Transform playerRef, int difficultyValue)
    {
        player = playerRef;
        playerInSight = true;
        
        //Scale the enemy's health with the game's difficulty
        if (difficultyValue >= 13)
        {
            health = 2;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Create a layer mask so the enemies debug traces dont collide with the enemy itself
        raycastLayerMask = LayerMask.GetMask("Default", "Player");
        StartCoroutine(CSpawnCooldown());
    }

    //Implement Interface function
    public void Hit()
    {
        health--;
        if (health <= 0)
        {
            //Play dying animation
            Destroy(gameObject);
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
                    StartCoroutine(CShootCooldown());
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
            
    }
}
