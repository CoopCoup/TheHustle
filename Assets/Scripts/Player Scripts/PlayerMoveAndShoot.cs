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
    [SerializeField] private GameObject firingPoint;

    //Create Firing Delay Co-routine
    IEnumerator coFire()
    {
        canFire = false;
       
        yield return new WaitForSeconds(.5f);
        canFire = true;
        animator.SetBool("IsFiring", false);
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
            animator.SetBool("IsFiring", true);
            //  Get which direction the player is facing to spawn the projectile correctly
            switch (direction)
            {
                case Direction.Left:
                    firingPoint.transform.position = new Vector3(-0.5f, 0.282f, 0); break;
                case Direction.Right:
                    firingPoint.transform.position = new Vector3(0.5f, 0.282f, 0); break;
                case Direction.Up:
                    firingPoint.transform.position = new Vector3(0.1565f, 0.6768f, 0); break;
                case Direction.Down:
                    firingPoint.transform.position = new Vector3(0.16f, -0.261f, 0); break;
                case Direction.UpRight:
                    firingPoint.transform.position = new Vector3(0.4895f, 0.6768f, 0); break;
                case Direction.UpLeft:
                    firingPoint.transform.position = new Vector3(-0.4895f, 0.6768f, 0); break;
                case Direction.DownRight:
                    firingPoint.transform.position = new Vector3(0.429f, -0.179f, 0); break;
                case Direction.DownLeft:
                    firingPoint.transform.position = new Vector3(-0.429f, -0.179f, 0); break;
            }
            StartCoroutine(coFire());
        }
    }


    // Move the player and set the animator bool parameter so that they animate
    private void MovePlayer()
    {
        rb.velocity = inputVector * moveSpeed;
        CheckPlayerFacing();
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
                    direction = Direction.Right;
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
