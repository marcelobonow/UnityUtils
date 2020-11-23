using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class CultureUtils : MonoBehaviour {

    public static CultureInfo GetCurrentCultureInfo()
    {
        SystemLanguage currentLanguage = Application.systemLanguage;
        CultureInfo correspondingCultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.EnglishName.Equals(currentLanguage.ToString()));
        return CultureInfo.CreateSpecificCulture(correspondingCultureInfo.TwoLetterISOLanguageName);
    }
}
