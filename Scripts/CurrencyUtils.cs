using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CurrencyUtils
{
  public static string GetPriceString(long cents) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", cents / 100m);
  public static string GetHumanPrice(long cents, string noValueString = "Sem Custo") =>
      cents == 0 && noValueString != null ? noValueString : GetPriceString(cents);

  public static bool ParsePrice(string text, out long cents)
  {
    if (text.StartsWith("r$"))
    {
      text = text.Remove(0, 2);
      text = text.Insert(0, "R$");
    }

    if (decimal.TryParse(text, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, CultureInfo.GetCultureInfo("pt-BR"), out var price))
    {
      cents = (long)(price * 100);
      return true;
    }
    cents = 0;
    return false;
  }
}
