using System;
using System.Collections;
using System.Collections.Generic;
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
}
