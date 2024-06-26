using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickReady : StateMachineBehaviour
{
    private GameObject jRef;
    private GameObject slotsRef;
    private SlotsScript slots;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        jRef = animator.gameObject;
        slotsRef = jRef.transform.parent.gameObject;
        slots = slotsRef.GetComponent<SlotsScript>();
        slots.JoystickReady();
    }


}
