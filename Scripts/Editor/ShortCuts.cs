using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;

public class ShortCuts{

    [MenuItem("Tools/Clear Console %#c")]
    public static void ClearConsole()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        Type logEntries = assembly.GetType("UnityEditorInternal.LogEntries");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
        clearConsoleMethod.Invoke(new object(), null);
    }
}
