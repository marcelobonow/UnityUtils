using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PointerHandler))]
public class PointerHandlerImageEffects : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color pressedColor = Color.white;
    [SerializeField] private float fadeDuration = 0.2f;

    ///Pegamos a cor normal da cor da imagem no awake
    private Color normalColor;
    private PointerHandler pointerHandler;
    private Coroutine coroutine;

    private void Awake()
    {
        pointerHandler = GetComponent<PointerHandler>();
        if (pointerHandler == null || image == null)
            return;

        pointerHandler.onPointerDown.Insert(OnPointerDown, this);
        pointerHandler.onPointerUp.Insert(OnPointerUp, this);
        pointerHandler.onPointerEnter.Insert(OnPointerEnter, this);
        pointerHandler.onPointerExit.Insert(OnPointerExit, this);

        normalColor = image.color;
    }

    private void OnPointerDown(PointerEventData eventData) => StartFade(image.color, pressedColor);
    private void OnPointerUp(PointerEventData eventData) => StartFade(image.color, normalColor);
    private void OnPointerEnter(PointerEventData eventData) => StartFade(image.color, hoverColor);
    private void OnPointerExit(PointerEventData eventData) => StartFade(image.color, normalColor);


    private void StartFade(Color startColor, Color endColor)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        if (gameObject.activeInHierarchy)
            coroutine = StartCoroutine(Fade(startColor, endColor));
        else
            image.color = endColor;
    }

    private IEnumerator Fade(Color startColor, Color endColor)
    {
        var initialTime = Time.time;
        var startPosition = transform.position;
        while (Time.time < initialTime + fadeDuration)
        {
            var step = (Time.time - initialTime) / fadeDuration;
            image.color = Color.Lerp(startColor, endColor, step);
            yield return null;
        }

        image.color = endColor;
    }
}
