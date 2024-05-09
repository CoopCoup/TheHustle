using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IColliders
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Transform player;

    //TEMPORARY - HAVE A REF TO THE PLAYER TO TEST IF EVERYTHING WORKS

    [SerializeField] private float sightRange = 50f;
    private bool playerInSight = false;
    private float fireDelay = 0.6f;
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
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }
    
    
    //Initialise the enemy in the room script, passing it a reference to the player
    public void Initialise(Transform playerRef)
    {
        player = playerRef;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, sightRange);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            //Check if player is within the lines of fire
            Vector2[] firingLines = { up, upRight, right, downRight, down, downLeft, left, upLeft };
            foreach (Vector2 line in firingLines)
            {
                hit = Physics2D.Raycast(transform.position, line, sightRange);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    Shoot(direction);
                }
            }
        }


    }
    
    //SHOOTING fucntionality
    private void Shoot(Vector2 direction)
    {
        Debug.Log("I SEE YOU");
    }
    
    
    
    
    private void FixedUpdate()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
