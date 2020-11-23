using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private bool toRight;
    [SerializeField] private bool up;
    [SerializeField] private float speed = 15f;
    private bool shouldMoveScrollRect;
    private bool isPressed;

    public void OnPointerDown(PointerEventData data)
    {
        shouldMoveScrollRect = true;
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        shouldMoveScrollRect = false;
        isPressed = false;
    }

    public void OnPointerExit(PointerEventData data)
    {
        shouldMoveScrollRect = false;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if(isPressed)
            shouldMoveScrollRect = true;
    }

    public void Update()
    {
        if(shouldMoveScrollRect)
        {
            if(scrollRect.horizontal)
            {
                var moveDistance = ((toRight ? speed : -speed) * Time.deltaTime * 1000f) / scrollRect.content.sizeDelta.x;
                if((toRight && scrollRect.normalizedPosition.x + moveDistance < 1) || (!toRight && scrollRect.normalizedPosition.x + moveDistance > 0))
                {
                    scrollRect.normalizedPosition += Vector2.right * moveDistance;
                }
            }
            if(scrollRect.vertical)
            {
                var moveDistance = (up ? speed : -speed) * Time.deltaTime * 1000f / scrollRect.content.sizeDelta.y;
                if((toRight && scrollRect.normalizedPosition.y + moveDistance < 1) || (!toRight && scrollRect.normalizedPosition.y + moveDistance > 0))
                {
                    scrollRect.normalizedPosition += Vector2.up * moveDistance;
                }
            }
        }
    }
}
