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

    public static Color HexToColor(string hex)
    {
        if (hex == null)
            return Color.white;
        hex = hex.Replace("0x", ""); ///Remove 0x no caso de estar formatado 0xFFFFFF
        hex = hex.Replace("#", ""); ///Remove # no caso de estar formatado #FFFFFF
        if (hex.Length != 6 && hex.Length != 8)
            return Color.white;

        var r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        ///Só checa alpha se tiver caracteres, se não assume completamente opaco
        var a = hex.Length == 8 ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;
        return new Color32(r, g, b, a);
    }
}
