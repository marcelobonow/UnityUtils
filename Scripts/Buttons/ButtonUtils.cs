using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ChangeNormalColor(this Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ChangeSelectedColor(this Button button, Color color)
    {
        var colors = button.colors;
        colors.selectedColor = color;
        button.colors = colors;
    }

}
