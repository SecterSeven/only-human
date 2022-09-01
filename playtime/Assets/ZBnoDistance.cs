using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBnoDistance : StateMachineBehaviour
{
     Transform target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        float distance = Vector2.Distance(target.position, animator.transform.position);
        if (distance >= 0)
        {
            animator.SetBool("isChasing", true);
            
        }else if (Player.health <= 0)
        {
            animator.SetBool("isChasing", false);
        }
    }
}
