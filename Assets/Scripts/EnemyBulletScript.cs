using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour, IColliders
{
    private Rigidbody2D rb;
    public float bulletSpeed;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    //Initialised by enemy
    public void Initialise(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed; 
    }


    //Implement interface method
    public void Hit()
    {
        Destroy(gameObject);
    }
    
    
    
    //Collision
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
        Destroy(this.gameObject);
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
