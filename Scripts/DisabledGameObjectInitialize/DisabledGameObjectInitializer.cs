using System;
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

        var rootObjects = new List<GameObject>();
        rootObjects.AddRange(scene.GetRootGameObjects());

        var dontDestroyOnLoadAccessor = new GameObject("DontDestroyOnLoadAccessor");
        DontDestroyOnLoad(dontDestroyOnLoadAccessor);
        rootObjects.AddRange(dontDestroyOnLoadAccessor.scene.GetRootGameObjects());

        foreach (var go in rootObjects)
            scripts.AddRange(go.GetComponentsInChildren<IDisabledGameObjectInitializable>(true));
        DestroyImmediate(dontDestroyOnLoadAccessor);
        foreach (var script in scripts)
        {
            try
            {
                script.Initialize();
            }
            catch (Exception exception)
            {
                Debug.LogError("Erro inicializando script: " + exception);
            }
        }
    }
}
