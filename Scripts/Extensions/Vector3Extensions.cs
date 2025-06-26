using UnityEngine;

using Range = UnityEngine.Random;

public static partial class ExtensionMethods
{
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
        result.x = Range.Range(min.x, max.x);
        result.y = Range.Range(min.y, max.y);
        result.z = Range.Range(min.z, max.z);
        return result;
    }

    public static Vector3 FromPlaneToVector3(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);
    public static Vector3 HorizontalPlaneRotation(this Quaternion rotation) => rotation.eulerAngles.HorizontalPlaneRotation();
    public static Vector3 HorizontalPlaneRotation(this Vector3 rotation)
    {
        rotation.y = 0;
        rotation.Normalize();
        return rotation;
    }

    public static Vector3 MultiplyElementByElement(this Vector3 thisVector3, Vector3 otherVector3) =>
        new Vector3(thisVector3.x * otherVector3.x, thisVector3.y * otherVector3.y, thisVector3.z * otherVector3.z);

    public static float DistanceOnPlane(this Vector3 vector3, Vector3 other)
    {
        vector3.y = 0;
        other.y = 0;
        return Vector3.Distance(vector3, other);
    }
    public static Vector2 ToVector2XZ(this Vector3 vector3) => new Vector2(vector3.x, vector3.z);

    public static Vector3 Abs(this Vector3 vector3) => new(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
    public static bool BiggerEveryComponent(this Vector3 a, Vector3 b) => a.x > b.x && a.y > b.y && a.z > b.z;
}
