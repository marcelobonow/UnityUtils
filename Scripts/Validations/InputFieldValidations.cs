using TMPro;
public static class InputFieldValidations
{
    
    public static void AutoMaskCPF(this TMP_InputField inputField)
    {
        inputField.onValueChanged.AddListener((s) => MaskCPF(ref inputField));
    }

    public static void RemoveAutoMaskCPF(this TMP_InputField inputField)
    {
        inputField.onValueChanged.RemoveListener((s) => MaskCPF(ref inputField));
    }

    public static void MaskCPF(ref TMP_InputField inputField)
    {
        var cpfNumber = inputField.text.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        if (cpfNumber.Length == 11)
        {
            inputField.text = TextValidations.MaskCPF(cpfNumber);
            inputField.MoveToEndOfLine(false, false);
        }
        else
            inputField.text = cpfNumber.ToString();
    }

}
