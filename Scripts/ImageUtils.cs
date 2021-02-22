using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageUtils
{
    ///Returns in order bottom left, bottom right, top right and top left
    public static List<Vector2> OrderPoints(List<Vector2> points)
    {
        var squareCenter = new Vector2(points.Sum(point => point.x), points.Sum(point => point.y)) / points.Count;
        return points.OrderBy(point => Mathf.Atan2(point.y - squareCenter.y, point.x - squareCenter.x)).ToList();
    }
}
