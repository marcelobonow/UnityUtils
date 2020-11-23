using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEndAnimation : MonoBehaviour
{
    [SerializeField] public StringActionDictionary onEnd;


    public void OnAnimationEnd(string eventName)
    {
        if (onEnd.ContainsKey(eventName))
            onEnd[eventName]?.Invoke();
    }
}
