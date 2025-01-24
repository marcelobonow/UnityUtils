using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class PriceInputMasking : TMP_InputField
{
    public Events<long> onChangeValue = new();
    public Events<long> onFinish = new();

    private void Awake()
    {
        onValueChanged.AddListener(text => OnValueChanged(text));
        onSubmit.AddListener(OnSubmit);
        onEndEdit.AddListener(OnSubmit);
    }

    private string RemoveLeadingZeroes(string text)
    {
        var strResult = new StringBuilder();

        var foundNumber = false;
        ///Remove todos os zeros da esquerda
        foreach (var element in text.ToCharArray())
        {
            if (!foundNumber && element != '0')
                foundNumber = true;

            if (foundNumber)
                strResult.Append(element);
        }
        return strResult.ToString().PadLeft(3, '0');
    }

    private void OnSubmit(string text)
    {
        var cents = OnValueChanged(text);
        onFinish.Trigger(cents);
    }

    private long OnValueChanged(string text)
    {
        var oldCaretPosition = this.caretPosition;
        var isLastPosition = oldCaretPosition == text.Length;

        string justNumbers = new string(text.Where(char.IsDigit).ToArray());
        long cents = 0;
        long.TryParse(justNumbers, out cents);

        if (justNumbers.Length <= 2)
        {
            this.SetTextWithoutNotify(justNumbers);
            return cents;
        }
        var masked = RemoveLeadingZeroes(justNumbers);

        masked = Regex.Replace(masked, @"(\d*)(\d{2})", "$1,$2");
        masked = Regex.Replace(masked, @"\B(?=(\d{3})+(\D))", ".");

        masked = "R$ " + masked;

        Console.WriteLine(masked);
        this.SetTextWithoutNotify(masked);

        if (isLastPosition)
            this.caretPosition = masked.Length;
        else
            this.caretPosition = oldCaretPosition;

        onChangeValue.Trigger(cents);
        return cents;
    }
}
