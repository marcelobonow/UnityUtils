using UnityEngine;
public class GameObjectUtils
{
    /// <summary>
    /// Retorna o caminho na hierarquia do unity do GameObject
    /// </summary>
    /// <param rewardName="go">Objeto a ser retornado a hierarquia </param>
    /// <returns></returns>
	public static string TransformFullPath(Transform transform)
    {
        string path = "/" + transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = "/" + transform.name + path;
        }
        return path;
    }
}
