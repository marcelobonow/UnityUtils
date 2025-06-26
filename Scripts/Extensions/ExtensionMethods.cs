using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Range = UnityEngine.Random;

public static partial class ExtensionMethods
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
    public static void ChangeAlpha(this Image image, float alpha)
    {
        var tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
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
    public static T GetRandom<T>(this IEnumerable<T> list) => list.ElementAt(Range.Range(0, list.Count()));
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

    public static Vector2 RandomVector2(Vector2 min, Vector2 max)
    {
        Vector2 result;
        result.x = Range.Range(min.x, max.x);
        result.y = Range.Range(min.y, max.y);
        return result;
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

    public static Vector2 XY(this Vector4 vector) => new Vector2(vector.x, vector.y);
    public static Vector2 ZW(this Vector4 vector) => new Vector2(vector.z, vector.w);
    public static Vector2 MultiplyAndSum(this Vector4 vector4, Vector2 vector2)
    {
        return vector4.XY() * vector2 + vector4.ZW();
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
    public static IEnumerator LocalSmoothRotation(this Transform transform, Quaternion rotation, float duration)
    {
        var initialTime = Time.time;
        var startRotation = transform.localRotation;
        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.localRotation = Quaternion.Lerp(startRotation, rotation, Mathf.SmoothStep(0f, 1f, step));
            yield return null;
        }
        transform.localRotation = rotation;
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
    public static IEnumerator LocalSmoothMovement(this Transform transform, Vector3 position, float duration, UnityAction onFinishMovement = null)
    {
        var initialTime = Time.time;
        var startPosition = transform.localPosition;
        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.localPosition = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0f, 1f, step));
            yield return null;
        }
        transform.localPosition = position;

        onFinishMovement?.Invoke();
    }
    public static IEnumerator SmoothMovement(this Transform transform, Vector3 position, float duration, UnityAction onFinishMovement = null)
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

        onFinishMovement?.Invoke();
    }
    public static IEnumerator SmoothMovementRotation(this Transform transform, Vector3 position, Quaternion rotation, float duration)
    {
        var initialTime = Time.time;
        var startPosition = transform.position;
        var startRotation = transform.rotation;

        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0f, 1f, step)),
                Quaternion.Lerp(startRotation, rotation, Mathf.SmoothStep(0f, 1f, step))
            );
            yield return null;
        }

        transform.SetPositionAndRotation(position, rotation);
    }

    public static IEnumerator SmoothMovementRotation(this Transform transform, Vector3 position, Quaternion rotation, float duration, AnimationCurve animationCurve)
    {
        var initialTime = Time.time;
        var startPosition = transform.position;
        var startRotation = transform.rotation;

        while (Time.time < initialTime + duration)
        {
            var step = (Time.time - initialTime) / duration;
            transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, position, animationCurve.Evaluate(step)),
                Quaternion.Lerp(startRotation, rotation, animationCurve.Evaluate(step))
            );
            yield return null;
        }

        transform.SetPositionAndRotation(position, rotation);
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

    public static string ToTitleCase(this string text) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());

    public static bool TryGetComponentInObjectOrParent<T>(this Transform transform, out T component) where T : Component
    {
        component = transform.GetComponent<T>();
        if (component == null && transform.parent != null)
            component = transform.parent.GetComponent<T>();

        return component != null;
    }

}