using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenericInputField : GenericInput
{
    private InputField InputField
    {
        get
        {
            if(inputField == null)
                inputField = GetComponent<InputField>();
            return inputField;
        }
        set
        {
            inputField = value;
        }
    }
    private InputField inputField;

    public override void SetInputField()
    {
        inputField = GetComponent<InputField>();
        if(inputField == null)
            throw new Exception("ComponentNotAdded");
    }

    public override void AddListener(UnityAction<string> listener)
    {
        InputField.onValueChanged.AddListener(listener);
    }
    public override void AddOnEndListener(UnityAction<string> listener)
    {
        InputField.onEndEdit.AddListener(listener);
    }
    public override Color GetSelectionColor()
    {
        return InputField.selectionColor;
    }

    public override void SetSelectionColor(Color color)
    {
        InputField.selectionColor = color;
    }

    public override void SetText(string text)
    {
        InputField.text = text;
    }

    public override string GetText()
    {
        return InputField.text;
    }

    public override int GetTextLenght()
    {
        return InputField.text.Length;
    }

    public override void SetCaretPosition(int position)
    {
        InputField.caretPosition = position;
    }

    public override bool IsFocused()
    {
        return InputField.isFocused;
    }

    public override void BlockInput()
    {
        InputField.enabled = false;
    }

    public override void EnableInput()
    {
        InputField.enabled = true;
    }

    public override bool IsInputEnabled()
    {
        return InputField.enabled;
    }
}
