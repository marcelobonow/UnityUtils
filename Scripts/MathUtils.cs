using System.Collections.Generic;
using UnityEngine;

public class MathUtils
{
    public static float NormalizeAngle(float angle)
    {
        return (angle % 360 + 360) % 360;
    }

    public static Vector3 DistancePointToLineSegment(Vector3 point, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;
        Vector3 ap = point - a;
        float t = Vector3.Dot(ap, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t); // garante que t fique no segmento
        Vector3 closestPoint = a + t * ab;
        return closestPoint - point;
    }

    public static int ClosestPointToLineSegment(IEnumerable<Vector3> points, Vector3 start, Vector3 end)
    {
        var closestPointIndex = 0;
        var closestPointDistance = Mathf.Infinity;
        var i = 0;
        foreach (var point in points)
        {
            var pointDistance = DistancePointToLineSegment(point, start, end).sqrMagnitude;
            if (pointDistance < closestPointDistance)
            {
                closestPointDistance = pointDistance;
                closestPointIndex = i;
            }
            i++;
        }

        return closestPointIndex;
    }

}
