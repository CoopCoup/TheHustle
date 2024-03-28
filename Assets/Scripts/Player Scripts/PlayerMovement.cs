using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //create our variables
    private Rigidbody2D rb;
    private Vector2 inputVector;
    private Vector2 recentInput;
    private Animator animator;
    [SerializeField] private float moveSpeed;
    private bool isDiagonal;

    // Enum of player directions to use in the animator
    public Direction direction;
    public enum Direction
    {
        Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
    }

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    
    }

    private void Start()
    {
        //Set the initial direction the player is facing
        direction = Direction.Right;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        Debug.Log(direction);
    }

    //Unity Event for the player's input
    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    // Move the player and set the animator bool parameter so that they animate
    private void MovePlayer()
    {
        rb.velocity = inputVector * moveSpeed;
        if (inputVector != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else animator.SetBool("IsMoving", false);
        CheckPlayerFacing();
    }

    //check which direction the player is facing
    private void CheckPlayerFacing()
    {

        // if the player is moving diagonally
        if ((inputVector.x != 0) & (inputVector.y != 0))
        {
            isDiagonal = true;
            #region Diagonal Direction Switch Statement
            //switch to get the players direction and set it in an enum
            switch (inputVector.x)
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
        /*#region Cardinal Direction Switch Statement
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
            #endregion*/

        }
    }
}
