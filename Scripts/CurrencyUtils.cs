using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CurrencyUtils
{
    public static string GetPriceString(long cents) => string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", cents / 100m);
    public static string GetHumanPrice(long cents, string noValueString = "Sem Custo") =>
        cents == 0 && noValueString != null ? noValueString : GetPriceString(cents);
}
