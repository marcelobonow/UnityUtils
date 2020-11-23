using System.Collections;
using System;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Script utilizado no inspector, para facilitar depuração (permitindo a remoção do player prefs).
/// </summary>
public class ClearPrefs : MonoBehaviour
{

    [MenuItem("Beupse/ClearPrefs")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Logger.Log("Deleted PlayerPrefs");
    }

    [MenuItem("Beupse/ClearCache")]
    public static void ClearCache()
    {
        Caching.ClearCache();
        Logger.Log("Deleted Cache");
    }
}
