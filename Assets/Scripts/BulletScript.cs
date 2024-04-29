using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletScript : MonoBehaviour, IColliders
{
    //Saving the rotation of the collision for each orientation of the bullet
    [SerializeField] private Vector3 rightUpColRot;
    [SerializeField] private Vector3 rightDownColRot;   
    [SerializeField] private Vector3 rightColRot;   
    [SerializeField] private Vector3 upColRot;   
    [SerializeField] private GameObject bulletCol;
    private bool firing = false;
    private Vector2 bulletAim;
    [SerializeField] private float bulletSpeed;
    private SpriteRenderer spriteRen;
    private Rigidbody2D rb;
    [SerializeField] private Sprite[] sprites;
    
    
    // ----------------------------------------------------------------------------------------
    //Awake
    // ----------------------------------------------------------------------------------------
    void Awake()
    {
        spriteRen = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    //get bullet direction from the gameobject spawning the bullet
    public void Instantiate(PlayerMovement.Direction bulletDirection, Vector3 bulletLocation)
    {
        transform.position = bulletLocation;
        switch (bulletDirection)
        {
            case PlayerMovement.Direction.Up:
                bulletAim = new Vector2(0, 1);
                spriteRen.sprite = sprites[0];
                break;

            case PlayerMovement.Direction.Right:
                bulletAim = new Vector2(1, 0);
                spriteRen.sprite = sprites[1];
                break;

            case PlayerMovement.Direction.Down:
                bulletAim = new Vector2(0, -1);
                spriteRen.sprite = sprites[0];
                break;

            case PlayerMovement.Direction.Left:
                bulletAim = new Vector2(-1, 0);
                spriteRen.sprite = sprites[1];
                break;

            case PlayerMovement.Direction.UpRight:
                bulletAim = new Vector2(1, 1);
                spriteRen.sprite = sprites[2];
                break;

            case PlayerMovement.Direction.UpLeft:
                bulletAim = new Vector2(-1, 1);
                spriteRen.sprite = sprites[3];
                break;

            case PlayerMovement.Direction.DownRight:
                bulletAim = new Vector2(1, -1);
                spriteRen.sprite = sprites[3];
                break;

            case PlayerMovement.Direction.DownLeft:
                bulletAim = new Vector2(-1, -1);
                spriteRen.sprite = sprites[2];
                break;

        }
        firing = true;
    }

    //Function to fire the bullet
    private void FireBullet()
    {
        rb.velocity = bulletAim.normalized * bulletSpeed;
    }


    // ----------------------------------------------------------------------------------------
    //Fixed Update
    // ----------------------------------------------------------------------------------------
    private void FixedUpdate()
    {
        if (firing)
        {
            FireBullet();
        }
    }

    //Destroy the bullet if it leaves the screen
    private void OnBecameInvisible()
    {
        if (!spriteRen.isVisible)
        {
            Destroy(gameObject);
        }
    }

    //Implement Interface fucntion
    public void OnHit(GameObject otherObject, float damage)
    {
        //what happens when the thing gets hit
        GameObject hitter = otherObject;

    }



    // ----------------------------------------------------------------------------------------
    //Update
    // ----------------------------------------------------------------------------------------
    void Update()
    {
        
    }
}