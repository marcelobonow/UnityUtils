using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// Um pequeno script utilizado para que o botão não fique como marcado após o clique
public class ButtonHighlightedSolution : MonoBehaviour, IPointerUpHandler
{
    private Selectable button;

    public void Start()
    {
        button = button ?? gameObject.GetComponent<Selectable>();
    }

    public void OnPointerUp(PointerEventData data)
    {
        button = button ?? gameObject.GetComponent<Selectable>();
        button.OnDeselect(null);
    }
}
