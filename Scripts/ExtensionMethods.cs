using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods
{
    private static System.Random rng = new System.Random();

    public static void Enable(this Image image)
    {
        image.color = Color.white;

        var texts = image.GetComponentsInChildren<Text>();
        foreach (var t in texts)
            t.color = Color.white;

        var images = image.GetComponentsInChildren<Image>();
        foreach (var i in images)
            if (i != image)
                i.color = Color.white;
    }
    public static void Disable(this Image image)
    {
        if (image == null)
        {
            Logger.LogError("null");
        }
        image.color = Color.gray;

        var texts = image.GetComponentsInChildren<Text>();
        foreach (var t in texts)
            t.color = Color.gray;

        var images = image.GetComponentsInChildren<Image>();
        foreach (var i in images)
            if (i != image)
                i.color = Color.gray;
    }
    public static void Clear(this Transform transform)
    {
        foreach (Transform child in transform)
            Object.Destroy(child.gameObject);
    }
    public static void Shuffle(this IList list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static T GetRandom<T>(this IList<T> list) => list[Random.Range(0, list.Count)];
    public static T GetLast<T>(this IList<T> list) => list[list.Count - 1];

    public static bool HasCharacter(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;
        else
        {
            foreach (var c in text)
            {
                if (c != ' ')
                    return true;
            }
            return false;
        }
    }

    public static string GetBetween(this string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start, End;
            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            End = strSource.IndexOf(strEnd, Start);
            return strSource.Substring(Start, End - Start);
        }

        return "";
    }

    public static Vector3 Clamp(this Vector3 vector3, Vector3 min, Vector3 max)
    {
        Vector3 result;
        result.x = Mathf.Clamp(vector3.x, min.x, max.x);
        result.y = Mathf.Clamp(vector3.y, min.y, max.y);
        result.z = Mathf.Clamp(vector3.z, min.z, max.z);
        return result;
    }

    public static Vector3 RandomVector3(Vector3 min, Vector3 max)
    {
        Vector3 result;
        result.x = Random.Range(min.x, max.x);
        result.y = Random.Range(min.y, max.y);
        result.z = Random.Range(min.z, max.z);
        return result;
    }

    public static Vector2 ToVector2XZ(this Vector3 vector3) => new Vector2(vector3.x, vector3.z);
    public static Vector3 FromPlaneToVector3(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);
    public static float DistanceOnPlane(this Vector3 vector3, Vector3 other)
    {
        vector3.y = 0;
        other.y = 0;
        return Vector3.Distance(vector3, other);
    }

    /// <summary>
    /// A = x, B = y, C = z;
    /// </summary>
    /// <param name="equation"></param>
    /// <returns>
    /// X = Positive
    /// Y = Negative
    /// </returns>
    public static Vector2 QuadraticFormula(this Vector3 equation)
    {
        Vector2 result = Vector2.zero;
        var a = equation.x;
        var b = equation.y;
        var c = equation.z;
        var delta = Mathf.Sqrt(b * b - 4 * a * c);
        result.x = (-b + delta) / (2 * a);
        result.y = (-b - delta) / (2 * a);
        return result;
    }

    public static IEnumerator AnimateValueChange(this TextMeshProUGUI text, int oldValue, int newValue, float duration)
    {
        var initialTime = Time.time;
        while (Time.time <= initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            var currentValue = Mathf.RoundToInt(Mathf.Lerp(oldValue, newValue, step));
            text.text = currentValue.ToString();
            yield return null;
        }
        text.text = newValue.ToString();
    }
    public static void DisableGameObjects(this ICollection<GameObject> gameObjects)
    {
        foreach (GameObject go in gameObjects)
        {
            if (go != null)
                go.SetActive(false);
        }
    }
    public static IEnumerator SmoothRotation(this Transform transform, Quaternion rotation, float duration)
    {
        var initialTime = Time.time;
        var startRotation = transform.rotation;
        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.rotation = Quaternion.Lerp(startRotation, rotation, Mathf.SmoothStep(0f, 1f, step));
            yield return null;
        }
        transform.rotation = rotation;
    }
    public static IEnumerator SmoothMovement(this Transform transform, Vector3 position, float duration)
    {
        var initialTime = Time.time;
        var startPosition = transform.position;
        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.position = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0f, 1f, step));
            yield return null;
        }
        transform.position = position;
    }

    public static IEnumerator SmoothMovementRotation(this Transform transform, Vector3 position, Quaternion rotation, float duration)
    {
        var initialTime = Time.time;
        var startPosition = transform.position;
        var startRotation = transform.rotation;

        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.position = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0f, 1f, step));
            transform.rotation = Quaternion.Lerp(startRotation, rotation, Mathf.SmoothStep(0f, 1f, step));
            yield return null;
        }

        transform.position = position;
        transform.rotation = rotation;
    }

    public static IEnumerator LerpTimeScale(float newTimeScale, float realTimeDuration)
    {
        var initialTime = Time.unscaledTime;
        var initialTimeScale = Time.timeScale;

        while (Time.unscaledTime < initialTime + realTimeDuration)
        {
            var step = (Time.unscaledTime - initialTime) / realTimeDuration;
            Time.timeScale = Mathf.Lerp(initialTimeScale, newTimeScale, step);
            yield return null;
        }
        Time.timeScale = newTimeScale;
    }
}