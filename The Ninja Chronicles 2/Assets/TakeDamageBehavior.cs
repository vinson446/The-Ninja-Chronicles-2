using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageBehavior : StateMachineBehaviour
{
    public float timer;
    float currentTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTimer = timer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTimer -= Time.deltaTime;

        if (currentTimer <= 0)
        {
            animator.SetTrigger("isIdle");
            currentTimer = timer;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
