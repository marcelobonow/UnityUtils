using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GenericTMPInput : GenericInput
{
    private TMP_InputField inputField;

    public override void SetInputField()
    {
        inputField = GetComponent<TMP_InputField>();
        if(inputField == null)
            throw new Exception("ComponentNotAdded");
    }

    public override void AddListener(UnityAction<string> listener)
    {
        inputField.onValueChanged.AddListener(listener);
    }
    public override void AddOnEndListener(UnityAction<string> listener)
    {
        inputField.onEndEdit.AddListener(listener);
    }

    public override void SetText(string text)
    {
        inputField.text = text;
    }

    public override string GetText()
    {
        return inputField.text;
    }

    public override int GetTextLenght()
    {
        return inputField.text.Length;
    }

    public override void SetCaretPosition(int position)
    {
        inputField.caretPosition = position;
    }

    public override Color GetSelectionColor()
    {
        return inputField.selectionColor;
    }

    public override void SetSelectionColor(Color color)
    {
        inputField.selectionColor = color;
    }

    public override bool IsFocused()
    {
        return inputField.isFocused;
    }

    public override void BlockInput()
    {
        inputField.enabled = false;
    }

    public override void EnableInput()
    {
        inputField.enabled = true;
    }

    public override bool IsInputEnabled()
    {
        return inputField.enabled;
    }
}
