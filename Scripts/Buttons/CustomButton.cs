using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool shouldMoveDown = true;
    [SerializeField] private bool shouldSpawnEventTrigger = true;
    [SerializeField] private float darknessFactor = 0.5f;
    private Selectable button;
    private Vector3 originalPosition;

    protected bool pressed;
    protected bool over;
    protected bool interactable;
    protected float timePressed;
    private float height;

    private bool pressedEffect;

    private bool initizalized = false;

    private Dictionary<int, Color> originalColors = new Dictionary<int, Color>();

    public Selectable Button
    {
        get
        {
            if (button == null)
                button = gameObject.GetComponent<Selectable>();
            return button;
        }

        set => button = value;
    }

    private void Reset()
    {
        pressed = false;
        over = false;
        timePressed = 0;
    }

    private void OnEnable()
    {
        Reset();
        if (!initizalized)
        {
            if (shouldSpawnEventTrigger && gameObject.GetComponent<EventTrigger>() == null)
                gameObject.AddComponent<EventTrigger>();

            StartCoroutine(WaitToGetInformations());
        }
    }


    protected IEnumerator WaitToGetInformations()
    {
        yield return new WaitUntil(() => gameObject.activeInHierarchy);
        yield return new WaitForEndOfFrame();
        GetInformations();
    }
    protected virtual void GetInformations()
    {
        originalPosition = gameObject.transform.localPosition;
        height = gameObject.GetComponent<RectTransform>().rect.height;
        interactable = gameObject.GetComponent<Selectable>().interactable;

        var parentGraphics = gameObject.GetComponents<Graphic>();
        var graphics = gameObject.GetComponentsInChildren<Graphic>(true).Where(x => !parentGraphics.Contains(x));
        foreach (var graphic in graphics)
            originalColors.Add(graphic.GetInstanceID(), graphic.color);
        initizalized = true;

        if (Button.interactable)
            ReleaseEffect();
        else
            PressEffect();
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
            pressed = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (button.interactable && data.button == PointerEventData.InputButton.Left)
            PressEffect();
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (button.interactable)
            ReleaseEffect();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (interactable && initizalized)
        {
            over = true;
            //if (pressed)
            //    PressEffect();
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (interactable)
        {
            over = false;
            //if (pressed)
            //    ReleaseEffect();
        }
    }

    public void SetEnableState(bool state)
    {
        if (state) Enable();
        else Disable();
    }

    public void Disable()
    {
        if (!pressedEffect)
            PressEffect();
        Button.interactable = false;
        interactable = false;
    }

    public void Enable()
    {
        Button.interactable = true;
        ReleaseEffect();
        interactable = true;
    }

    public bool IsActive() => interactable;

    protected virtual void PressEffect()
    {
        pressedEffect = true;
        timePressed = Time.time;

        var graphics = GetChildren();
        foreach (var graphic in graphics)
        {
            if (originalColors.ContainsKey(graphic.GetInstanceID()))
            {
                Color.RGBToHSV(graphic.color, out var h, out var s, out var v);
                v = v > darknessFactor ? darknessFactor : v;
                s = s > darknessFactor ? darknessFactor : s;
                graphic.color = Color.HSVToRGB(h, s, v);
            }
        }
        if (shouldMoveDown)
            gameObject.transform.localPosition = new Vector2(transform.localPosition.x, originalPosition.y - height / 100f);
    }
    protected virtual void ReleaseEffect()
    {
        pressedEffect = false;
        timePressed = 0;
        ///pega os componentes apenas nos filhos
        var graphics = GetChildren();
        foreach (var graphic in graphics)
        {
            if (originalColors.ContainsKey(graphic.GetInstanceID()))
                graphic.color = originalColors[graphic.GetInstanceID()];
        }

        if (shouldMoveDown)
            transform.localPosition = originalPosition;
    }

    protected IEnumerable<Graphic> GetChildren()
    {
        var parentGraphics = gameObject.GetComponents<Graphic>();
        return gameObject.GetComponentsInChildren<Graphic>(true).Where(x => !parentGraphics.Contains(x));
    }

    public void FadeEffect(bool fadeOut)
    {
        var graphics = gameObject.GetComponentsInChildren<Graphic>(true);

        if (fadeOut)
        {
            foreach (var graphic in graphics)
            {
                Color.RGBToHSV(graphic.color, out var h, out var s, out var v);
                v = v > darknessFactor ? darknessFactor : v;
                graphic.color = Color.HSVToRGB(h, s, v);
            }
        }
        else
        {
            foreach (var graphic in graphics)
            {
                Color.RGBToHSV(graphic.color, out var h, out var s, out var v);
                v = 1;
                graphic.color = Color.HSVToRGB(h, s, v);
            }
        }
    }

    public void ResetButtonPosition()
    {
        ///Existe a possibilidade do botão ser resetado antes de ter sido inicializado, nesse caso não se altera a posição.
        if (!initizalized)
            originalPosition = transform.localPosition;
        else if (shouldMoveDown)
            transform.localPosition = originalPosition;
    }

    public float GetTimePressed()
    {
        return timePressed;
    }

    public void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }
}
