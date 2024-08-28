using System;

public static class TextValidations
{

    public static bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }

    public static bool ValidateCPF(string text)
    {
        var number = text.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);

        return IsDigitsOnly(number) && number.Length == 11;
    }

    public static bool ValidateEmail(string str)
    {
        var trimmed = str.Trim();
        var containSpace = str.Split('@').Length > 1;
        return (trimmed != null && trimmed != "" && containSpace);
    }

    public static string MaskCPF(string text)
    {
        return Convert.ToUInt64(text).ToString(@"000\.000\.000\-00");
    }

}
