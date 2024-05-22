using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsDone : StateMachineBehaviour
{
    private GameObject eRef;
    private GameObject slotsRef;
    private SlotsScript slots;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        eRef = animator.gameObject;
        slotsRef = eRef.transform.parent.gameObject;
        slots = slotsRef.GetComponent<SlotsScript>();
        slots.SlotsFinished();
    }
}
