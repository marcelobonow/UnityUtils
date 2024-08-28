using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuickMacros : EditorWindow
{

    private static string macro = string.Empty;
    ///Dictionary com uma string que corresponde a função.
    private static Dictionary<string, Func<string[], int>> functionSignatures;
    private static bool hasInit = false;

    [MenuItem("Beupse/Macros #q")]
    private static void Init()
    {
        CreateInstance<QuickMacros>().ShowUtility();
        functionSignatures = new Dictionary<string, Func<string[], int>>();
        functionSignatures.Add("remove_multiples_highlighted", RemoveMultipleHighlighted);
        functionSignatures.Add("revert_prefabs", RevertPrefabs);
        functionSignatures.Add("apply_prefabs", ApplyPrefabs);
        functionSignatures.Add("set_prefs", SetPlayerPrefs);
        functionSignatures.Add("centralize", Centralize);
        functionSignatures.Add("circularize", Circularize);
        hasInit = true;
    }

    private void OnGUI()
    {
        if (!hasInit)
            Init();
        macro = EditorGUILayout.TextField(macro, GUILayout.ExpandHeight(true));
        var e = Event.current;
        if (e.type == EventType.KeyDown && (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter))
            Execute();
        if (GUILayout.Button("Execute Macro"))
            Execute();
    }

    private void Execute()
    {
        var args = macro.Split(' ');
        if (functionSignatures.ContainsKey(args[0]))
            functionSignatures[args[0]].Invoke(args);
        else
            Debug.LogWarning("Invalid command");
    }

    public static int RemoveMultipleHighlighted(string[] args)
    {
        var gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (var gameObject in gameObjects)
        {
            var buttonsHighlighted = gameObject.GetComponentsInChildren<ButtonHighlightedSolution>();
            Logger.Log("buttons highlighted length: " + buttonsHighlighted.Length);
            if (buttonsHighlighted.Length > 1)
            {
                Logger.Log("Executando");
                for (var i = 0; i < buttonsHighlighted.Length - 1; i++)
                {
                    DestroyImmediate(buttonsHighlighted[i]);
                }
            }
        }
        return 0;
    }
    public static int RevertPrefabs(string[] args)
    {
        var selection = Selection.gameObjects;
        if (selection.Length > 0)
        {
            foreach (var go in selection)
            {
                Debug.Log("Revertendo: " + go.name);
                PrefabUtility.RevertPrefabInstance(go, InteractionMode.AutomatedAction);
            }
            Debug.Log("Revertido prefabs");
            return 0;
        }
        return -1;
    }
    public static int ApplyPrefabs(string[] args)
    {
        var selection = Selection.gameObjects;
        if (selection.Length > 0)
        {
            foreach (var go in selection)
            {
                Debug.Log("Aplicando: " + go.name);
                PrefabUtility.ApplyPrefabInstance(go, InteractionMode.UserAction);
            }
            Debug.Log("Revertido prefabs");
            return 0;
        }
        return -1;
    }

    public static int Centralize(string[] args)
    {
        var selection = Selection.gameObjects;
        if (selection.Length <= 0)
            return -1;
        Centralize(selection[0]);
        return 0;
    }

    public static int Circularize(string[] args)
    {
        var selection = Selection.gameObjects;
        var objectsOrdered = selection.OrderBy(selected => selected.name).ToList();
        if (selection.Length <= 0)
            return -1;
        for (int i = 0; i < objectsOrdered.Count; i++)
        {
            ///Máximo 10
            var maxQuantity = int.Parse(args[1]);
            var radius = float.Parse(args[2]);
            var offset = args.Length >= 4 ? float.Parse(args[3]) : 0f;
            var degree = 360 / maxQuantity * i * Mathf.Deg2Rad;
            objectsOrdered[i].transform.localPosition = new Vector2(Mathf.Cos(degree + offset) * radius, Mathf.Sin(degree + offset) * radius);
        }
        Debug.Log($"Colocou {objectsOrdered.Count} objetos em circulo");
        return 0;
    }

    public static int SetPlayerPrefs(string[] args)
    {
        if (args.Length < 4)
        {
            Debug.LogWarning("Parameters missing");
            return -1;
        }
        if (args[2] == "")
        {
            Debug.LogWarning("Value to set is invalid");
            return -1;
        }

        switch (args[1])
        {
            case "int": PlayerPrefs.SetInt(args[2], int.Parse(args[3])); break;
            case "float": PlayerPrefs.SetFloat(args[2], float.Parse(args[3])); break;
            case "string": PlayerPrefs.SetString(args[2], args[3]); break;
            default: Debug.LogWarning($"{args[2]} is not a valid parameter"); return -1;
        }
        Debug.Log($"Player pref {args[2]} was set with value {args[3]}");
        return 0;
    }

    private static GameObject Centralize(GameObject gameObject)
    {
        var meshFilters = gameObject.GetComponentsInChildren<MeshFilter>(true);
        var maxBounds = Vector3.negativeInfinity;
        var minBounds = Vector3.positiveInfinity;
        foreach (var meshFilter in meshFilters)
        {
            var bounds = meshFilter.sharedMesh.bounds;
            var localPosition = meshFilter.transform.position;
            var vertices = meshFilter.sharedMesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                maxBounds = Vector3.Max(maxBounds, meshFilter.transform.TransformPoint(vertices[i]));
                minBounds = Vector3.Min(minBounds, meshFilter.transform.TransformPoint(vertices[i]));
            }
        }

        var center = (maxBounds + minBounds) / 2f;
        var newGameObject = new GameObject();
        newGameObject.name = gameObject.name;
        gameObject.transform.parent = newGameObject.transform;
        gameObject.transform.position -= center;
        newGameObject.transform.position = Vector3.zero;

        var collider = gameObject.AddComponent<BoxCollider>();
        collider.center = center;
        collider.size = (maxBounds - minBounds);
        return newGameObject;
    }
}
