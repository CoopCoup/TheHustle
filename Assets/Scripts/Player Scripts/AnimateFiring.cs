using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Build;
using UnityEngine;

public class AnimateFiring : StateMachineBehaviour
{
    public Sprite upSprite, upRightSprite, rightSprite, downRightSprite, downSprite;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement playerScript = animator.GetComponent<PlayerMovement>();
        
        switch (playerScript.direction)
        {
            case PlayerMovement.Direction.Right:
                playerScript.spriteRen.sprite = rightSprite; break; 
            case PlayerMovement.Direction.Up:
                playerScript.spriteRen.sprite = upSprite;
                UnityEngine.Debug.Log("checking"); break;
            case PlayerMovement.Direction.Down:
                playerScript.spriteRen.sprite = downSprite; break;
            case PlayerMovement.Direction.UpRight:
                playerScript.spriteRen.sprite = upRightSprite; break;
            case PlayerMovement.Direction.DownRight:
                playerScript.spriteRen.sprite = downRightSprite; break;
            case PlayerMovement.Direction.Left:
                playerScript.spriteRen.sprite = rightSprite; break;
            case PlayerMovement.Direction.UpLeft:
                playerScript.spriteRen.sprite = upRightSprite; break;
            case PlayerMovement.Direction.DownLeft:
                playerScript.spriteRen.sprite = downRightSprite; break;
        }
        
        
    }
}
