using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IColliders
{
    public GameObject bulletPrefab;
    private Transform player;
    private LayerMask raycastLayerMask;
    

    [SerializeField] private float sightRange = 100f;
    private bool playerInSight = false;
    private bool canFire = true;

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
    

    IEnumerator CTestTimer()
    {
        //DELETE WHEN DONE!
        yield return new WaitForSeconds(5f);
        GameObject playerInstance = GameObject.FindGameObjectWithTag("Player");
        player = playerInstance.transform;
        playerInSight = true;
    }

    
    //Initialise the enemy in the room script, passing it a reference to the player
    public void Initialise(Transform playerRef)
    {
        player = playerRef;
        playerInSight = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //TEMPORARY - HAVE A REF TO THE PLAYER TO TEST IF EVERYTHING WORKS
        StartCoroutine(CTestTimer());

        //Create a layer mask so the enemies debug traces dont collide with the enemy itself
        raycastLayerMask = LayerMask.GetMask("Default", "Player");

    }

    //Implement Interface function
    public void Hit()
    {
        Debug.Log("Enemy Hit!");
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
            Debug.Log(hit.rigidbody);
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
        Debug.Log("I SEE YOU");
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        EnemyBulletScript bulletRef = bulletInstance.GetComponent<EnemyBulletScript>();
        bulletRef.Initialise(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInSight)
        {
            CheckLineOfSight();
        }
            
    }
}
