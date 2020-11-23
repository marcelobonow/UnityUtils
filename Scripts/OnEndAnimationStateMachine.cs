using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEndAnimationStateMachine : StateMachineBehaviour
{
    [SerializeField] private string eventName;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var onEndAnimation = animator.GetComponent<OnEndAnimation>();
        if (onEndAnimation != null)
            onEndAnimation.OnAnimationEnd(eventName);
    }
}
