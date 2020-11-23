using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCoroutines
{
    private static IEnumerator DoAfterTime(float time, Action effect)
    {
        yield return WaitForSeconds(time);
        effect?.Invoke();
    }
    public static void DoAfterTime(float time, Action effect, MonoBehaviour mono) => mono.StartCoroutine(DoAfterTime(time, effect));

    private static IEnumerator DoAfterRealTime(float time, Action effect)
    {
        yield return WaitForSecondsRealtime(time);
        effect?.Invoke();
    }

    public static IEnumerator WaitForSeconds(float seconds)
    {
        var startTime = Time.time;
        while (Time.time < startTime + seconds)
            yield return null;
    }

    public static IEnumerator WaitForSecondsRealtime(float seconds)
    {
        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + seconds)
            yield return null;
    }
}
