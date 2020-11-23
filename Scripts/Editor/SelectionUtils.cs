using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectionUtils
{
    [MenuItem("Tools/Disable Selected Colliders")]
    public static void DisableColliders()
    {
        EnableSelectedColliders(false);
    }

    [MenuItem("Tools/Enable Selected Colliders")]
    public static void EnableColliders()
    {
        EnableSelectedColliders();
    }

    private static void EnableSelectedColliders(bool enabled = true)
    {
        GameObject[] selected = Selection.gameObjects;
        foreach(var obj in selected)
        {
            var colliders = obj.GetComponents<Collider>();
            foreach (var c in colliders)
                c.enabled = enabled;
            var childrenColliders = obj.GetComponentsInChildren<Collider>();
            foreach (var c in childrenColliders)
                c.enabled = enabled;
        }
    }

}
