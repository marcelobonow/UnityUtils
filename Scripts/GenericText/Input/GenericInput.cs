using UnityEngine;
using UnityEngine.Events;

public abstract class GenericInput : MonoBehaviour
{
    public abstract void SetInputField();

    public abstract void AddListener(UnityAction<string> listener);
    public abstract void AddOnEndListener(UnityAction<string> listener);

    public abstract string GetText();
    public abstract void SetText(string text);

    public abstract Color GetSelectionColor();
    public abstract void SetSelectionColor(Color color);

    public abstract int GetTextLenght();
    public abstract void SetCaretPosition(int position);
    public abstract bool IsFocused();

    public abstract void BlockInput();
    public abstract void EnableInput();
    public abstract bool IsInputEnabled();
}
