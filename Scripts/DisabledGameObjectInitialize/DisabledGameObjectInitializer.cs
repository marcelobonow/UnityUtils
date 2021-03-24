using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisabledGameObjectInitializer : MonoBehaviour
{
    private void Awake()
    {
        var scripts = new List<IDisabledGameObjectInitializable>();
        var scene = SceneManager.GetActiveScene();

        var rootObjects = scene.GetRootGameObjects();

        foreach (var go in rootObjects)
            scripts.AddRange(go.GetComponentsInChildren<IDisabledGameObjectInitializable>(true));
        foreach (var script in scripts)
            script.Initialize();
    }
}
