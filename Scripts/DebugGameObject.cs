using UnityEngine;
public class DebugGameObject : MonoBehaviour {
    /// <summary>
    /// Retorna o caminho na hierarquia do unity do GameObject
    /// </summary>
    /// <param rewardName="go">Objeto a ser retornado a hierarquia </param>
    /// <returns></returns>
	public static string GameObjectPath(GameObject go)
    {
        string path = "/" + go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = "/" + go.name + path;
        }
        return path;
    }
}
