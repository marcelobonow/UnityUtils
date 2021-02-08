using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Events<PointerEventData> onPointerDown = new Events<PointerEventData>();
    public Events<PointerEventData> onPointerUp = new Events<PointerEventData>();
    public Events<PointerEventData> onPointerClick = new Events<PointerEventData>();
    public Events<PointerEventData> onPointerEnter = new Events<PointerEventData>();
    public Events<PointerEventData> onPointerExit = new Events<PointerEventData>();

    public void OnPointerDown(PointerEventData pointerEventData) => onPointerDown.Trigger(pointerEventData);
    public void OnPointerUp(PointerEventData pointerEventData) => onPointerUp.Trigger(pointerEventData);
    public void OnPointerClick(PointerEventData eventData) => onPointerClick.Trigger(eventData);

    public void OnPointerEnter(PointerEventData eventData) => onPointerEnter.Trigger(eventData);
    public void OnPointerExit(PointerEventData eventData) => onPointerExit.Trigger(eventData);
}
