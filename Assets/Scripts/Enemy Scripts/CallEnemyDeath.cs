using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEnemyDeath : StateMachineBehaviour
{
    //ref to the enmy script
    private EnemyScript enemyScript;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ref to the enemy secured
        GameObject enemy = animator.gameObject;
        enemyScript = enemy.GetComponent<EnemyScript>();    
    }

}
