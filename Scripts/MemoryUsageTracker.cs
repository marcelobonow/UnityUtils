using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryUsageTracker : MonoBehaviour {

    private void Start() => StartCoroutine(TrackMemory());

    private IEnumerator TrackMemory()
    {
        while(true)
        {
            Logger.Log("Memory Usage: " + (System.GC.GetTotalMemory(true)/(1024 * 1024)) + "MB");
            yield return new WaitForSeconds(1);
        }
    }
}
