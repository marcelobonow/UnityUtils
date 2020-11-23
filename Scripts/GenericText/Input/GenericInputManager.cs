using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericInputManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] protected GenericInput genericInput;

    private bool hasTyped;
    private string originalText;

    private Action<string> OnMessageSend;

    public void Awake()
    {
        if (genericInput == null)
        {
            genericInput = GetComponent<GenericInput>();
            if (genericInput == null)
                throw new Exception("ComponentNotAdded");
        }
        genericInput.SetInputField();
        genericInput.AddListener(OnType);
        originalText = genericInput.GetText();
        SetEndMethod();
    }
    protected virtual void SetEndMethod()
    {
        genericInput.AddOnEndListener(TrySendMessageText);
    }

    public string GetInputText()
    {
        var text = genericInput.GetText();
        return text == originalText ? "" : text;
    }

    public void TrySendMessageText(string message)
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            SendMessageText(message);
    }
    public void SendMessageText(string message)
    {
        if (OnMessageSend != null)
            OnMessageSend(message);
        genericInput.SetText(string.Empty);
        hasTyped = false;
    }
    public void Reset()
    {
        if (originalText == null)
            originalText = genericInput.GetText();
        genericInput.SetText(originalText);
        hasTyped = false;
    }

    public void AddOnMessageSendListener(Action<string> OnMessageSend)
    {
        this.OnMessageSend += OnMessageSend;
    }

    public void RemoveOnMessageSendListener(Action<string> OnMessageSend)
    {
        this.OnMessageSend -= OnMessageSend;
    }
    public void OnSelect(BaseEventData data)
    {
        StartCoroutine(DisableHighlight());
        OnFocus();
    }
    public void OnDeselect(BaseEventData data)
    {
        OnLoseFocus();
    }

    protected void OnFocus()
    {
        if (!hasTyped)
            genericInput.SetText(string.Empty);
    }
    protected void OnLoseFocus()
    {
        if (!hasTyped || genericInput.GetText() == string.Empty)
        {
            genericInput.SetText(originalText);
            hasTyped = false;
        }
    }
    protected void OnType(string data)
    {
        hasTyped = true;
    }

    private IEnumerator DisableHighlight()
    {
        //Get original selection color
        var originalTextColor = genericInput.GetSelectionColor();
        //Remove alpha
        originalTextColor.a = 0f;

        //Apply new selection color without alpha
        genericInput.SetSelectionColor(originalTextColor);

        //Wait one Frame(MUST DO THIS!)
        yield return null;

        //Change the caret pos to the end of the text
        genericInput.SetCaretPosition(genericInput.GetTextLenght());

        //Return alpha
        originalTextColor.a = 1f;

        //Apply new selection color with alpha
        genericInput.SetSelectionColor(originalTextColor);
    }
}
