using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Events<PointerEventData> onPointerDown = new Events<PointerEventData>();
    public Events<PointerEventData> onPointerUp = new Events<PointerEventData>();

    public void OnPointerDown(PointerEventData pointerEventData) => onPointerDown.Trigger(pointerEventData);
    public void OnPointerUp(PointerEventData pointerEventData) => onPointerUp.Trigger(pointerEventData);
}
