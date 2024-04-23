using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class AnimateFiring : StateMachineBehaviour
{
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
                animator.PlayInFixedTime("PlayerFiring", -1, 0); break;
            case PlayerMovement.Direction.Up:
                animator.PlayInFixedTime("PlayerFiring", -1, 0.75f); break;
            case PlayerMovement.Direction.Down:
                animator.PlayInFixedTime("PlayerFiring", -1, 1); break;
            case PlayerMovement.Direction.UpRight:
                animator.PlayInFixedTime("PlayerFiring", -1, 0.5f); break;
            case PlayerMovement.Direction.DownRight:
                animator.PlayInFixedTime("PlayerFiring", -1, 0.25f); break;
            case PlayerMovement.Direction.Left:
                animator.PlayInFixedTime("PlayerFiring", -1, 0); break;
            case PlayerMovement.Direction.UpLeft:
                animator.PlayInFixedTime("PlayerFiring", -1, 0.5f); break;
            case PlayerMovement.Direction.DownLeft:
                animator.PlayInFixedTime("PlayerFiring", -1, 0.25f); break;
        }
        
        
    }
}
