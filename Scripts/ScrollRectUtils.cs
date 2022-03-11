using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ScrollRectUtils
{
    public static void AlignRectHorizontal(this ScrollRect scrollRect, Transform child)
    {
        var viewportLocalPosition = scrollRect.viewport.localPosition;
        var childLocalPosition = child.localPosition;
        var newScrollContentPos = new Vector2(-(viewportLocalPosition.x + childLocalPosition.x), -(viewportLocalPosition.y + childLocalPosition.y));
        newScrollContentPos.y = scrollRect.content.localPosition.y;
        scrollRect.content.localPosition = newScrollContentPos;
    }
}
