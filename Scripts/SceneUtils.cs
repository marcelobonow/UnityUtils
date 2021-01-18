using System;
using UnityEngine.SceneManagement;

public class SceneUtils
{
    public static string GetSceneName(int sceneBuildIndex)
    {
        var scenePath = SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex);
        var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
        var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
        var sceneNameLength = sceneNameEnd - sceneNameStart;
        return scenePath.Substring(sceneNameStart, sceneNameLength);
    }
}
