using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCoroutines
{
    public static Coroutine DoAfterTime(float time, Action effect, MonoBehaviour mono) => mono.StartCoroutine(DoAfterTime(time, effect));
    private static IEnumerator DoAfterTime(float time, Action effect)
    {
        yield return WaitForSeconds(time);
        effect?.Invoke();
    }

    public static Coroutine DoAfterFrames(int frames, Action effect, MonoBehaviour mono) => mono.StartCoroutine(DoAfterFrames(frames, effect));
    private static IEnumerator DoAfterFrames(int frames, Action effect)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        effect?.Invoke();
    }

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

    public static Coroutine DoOnEndOfFrame(Action effect, MonoBehaviour mono) => mono.StartCoroutine(DoOnEndOfFrame(effect));
    public static IEnumerator DoOnEndOfFrame(Action effect)
    {
        yield return new WaitForEndOfFrame();
        effect.Invoke();
    }

    ///Can call at the end two times
    public static Coroutine Animate(float duration, Action<float> effect, Action onFinish, MonoBehaviour mono) => mono.StartCoroutine(Animate(duration, effect, onFinish));
    public static IEnumerator Animate(float duration, Action<float> effect, Action onFinish)
    {
        var startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            var step = (Time.time - startTime) / duration;
            effect?.Invoke(step);
            yield return null;
        }
        onFinish?.Invoke();
    }
}
