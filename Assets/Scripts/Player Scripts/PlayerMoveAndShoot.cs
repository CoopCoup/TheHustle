using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //create our variables
    private Rigidbody2D rb;
    private SpriteRenderer spriteRen;
    private Vector2 inputVector;
    private float inputX;
    private Animator animator;
    [SerializeField] private float moveSpeed;
    private bool isRight;
    private bool canFire = true;
    private bool canMove = true;
    private bool isDead = false;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Vector3 upFiringPoint; 
    [SerializeField] private Vector3 flippedUpFiringPoint; 
    [SerializeField] private Vector3 upRightFiringPoint; 
    [SerializeField] private Vector3 rightFiringPoint; 
    [SerializeField] private Vector3 downRightFiringPoint; 
    [SerializeField] private Vector3 downFiringPoint;
    [SerializeField] private Vector3 flippedDownFiringPoint;
    [SerializeField] private Vector3 downLeftFiringPoint;
    [SerializeField] private Vector3 leftFiringPoint;
    [SerializeField] private Vector3 upLeftFiringPoint;


    //Create Firing Delay Co-routine
    IEnumerator coFire()
    {
        canFire = false;
        GameObject newBullet = Instantiate(bulletPrefab);
        BulletScript bulletLogic = newBullet.GetComponent<BulletScript>();
        bulletLogic.Instantiate(direction, firingPoint.transform.position);
        yield return new WaitForSeconds(.25f);
        animator.SetBool("IsFiring", false);

        if (!isDead)
        {
            canMove = true;
        }
        
    }

    IEnumerator coFireCooldown()
    {
        yield return new WaitForSeconds(.5f);
        canFire = true;
    }
    
    
    // Enum of player directions to use in the animator
    public Direction direction;
    public enum Direction
    {
        Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
    }

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();    
    }

    private void Start()
    {
        //Set the initial direction the player is facing
        direction = Direction.Right;
        inputX = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        AnimatePlayer();

    }

    //Unity Event for the player's input
    public void OnMove(InputAction.CallbackContext context)
    {
            inputVector = context.ReadValue<Vector2>();
            inputX = Mathf.RoundToInt(inputVector.x);
    }

    //Unity Event for the player' firing input
    public void OnFire(InputAction.CallbackContext context)
    {
        if (canFire)
        {
            canMove = false;
            StopPlayer();
            animator.SetBool("IsFiring", true);
            
            //  Get which direction the player is facing to spawn the projectile correctly
            switch (direction)
            {
                case Direction.Left:
                    animator.Play("PlayerFiringRight");
                    firingPoint.transform.position = transform.position + leftFiringPoint; break;
                case Direction.Right:
                    animator.Play("PlayerFiringRight");
                    firingPoint.transform.position = transform.position + rightFiringPoint; break;
                case Direction.Up:
                    animator.Play("PlayerFiringUp");
                    if (spriteRen.flipX)
                    {
                        firingPoint.transform.position = transform.position + flippedUpFiringPoint; break;
                    }
                    else
                    {
                        firingPoint.transform.position = transform.position + upFiringPoint; break;
                    }
                case Direction.Down:
                    animator.Play("PlayerFiringDown");
                    if (spriteRen.flipX)
                    {
                        firingPoint.transform.position = transform.position + flippedDownFiringPoint; break;
                    }
                    else
                    {
                        firingPoint.transform.position = transform.position + downFiringPoint; break;
                    }
                case Direction.UpRight:
                    animator.Play("PlayerFiringUpRight");
                    firingPoint.transform.position = transform.position + upRightFiringPoint; break;
                case Direction.UpLeft:
                    animator.Play("PlayerFiringUpRight");
                    firingPoint.transform.position = transform.position + upLeftFiringPoint; break;
                case Direction.DownRight:
                    animator.Play("PlayerFiringDownRight");
                    firingPoint.transform.position = transform.position + downRightFiringPoint; break;
                case Direction.DownLeft:
                    animator.Play("PlayerFiringDownRight");
                    firingPoint.transform.position = transform.position + downLeftFiringPoint; break;
            }
            StartCoroutine(coFire());
            StartCoroutine(coFireCooldown());
        }
    }


    // Move the player and set the animator bool parameter so that they animate
    private void MovePlayer()
    {
        if (canMove)
        {
            rb.velocity = inputVector * moveSpeed;
            CheckPlayerFacing();
        }
        
    }

    private void StopPlayer()
    {
        rb.velocity = Vector2.zero;
    }

    //check which direction the player is facing
    private void CheckPlayerFacing()
    {
        //Get whether the player is facing left or right
        if (inputX > 0)
        {
            isRight = true;
        }
        else if (inputX < 0)
        {
            isRight = false;
        }

        // if the player is moving diagonally
        if ((inputVector.x != 0) & (inputVector.y != 0))
        {
            #region Diagonal Direction Switch Statement
            //switch to get the players direction and set it in an enum
            switch (inputX)
            {
                case 1:
                    if (inputVector.y > 0)
                    {
                        direction = Direction.UpRight;
                    }
                    if (inputVector.y < 0)
                    {
                        direction = Direction.DownRight;
                    }
                    break;
                case -1:
                    if (inputVector.y > 0)
                    {
                        direction = Direction.UpLeft;
                    }
                    if (inputVector.y < 0)
                    {
                            direction = Direction.DownLeft;
                    }
                    break;

            }
            #endregion
        }
        else
        {
            #region Cardinal Direction Switch Statement
        //switch to get the players direction and set it in an enum
        switch (inputVector.x)
        {
            case 0:
                if (inputVector.y > 0)
                {
                    direction = Direction.Up;
                }
                if (inputVector.y < 0)
                {
                    direction = Direction.Down;
                }
                if (inputVector.y == 0)
                {
                    if (spriteRen.flipX)
                        {
                            direction = Direction.Left;
                        } else { direction = Direction.Right; }
                }
                break;
            case 1:
                if (inputVector.y == 0)
                {
                    direction = Direction.Right;
                }
                break;
            case -1:
                if (inputVector.y == 0)
                {
                    direction = Direction.Left;
                }
                break;

        }
            #endregion

        }
    }

    //animate the player
    private void AnimatePlayer()
    {
        if (!isRight)
        {
            spriteRen.flipX = true;
        }
        else spriteRen.flipX = false;

        //set movement animator bool
        if (inputVector != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else animator.SetBool("IsMoving", false);
        
        //REMEMBER! When the firing point game object is added, remember to flip it here along with the player sprite 
    }

    
}