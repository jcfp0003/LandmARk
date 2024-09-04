using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

// https://docs.unity3d.com/Packages/com.unity.localization@1.3/manual/Scripting.html#make-a-basic-locale-selection-menu
public class LocalePickerDropdown : MonoBehaviour
{
    static readonly string PREFS_LOCALE_ID = "Locale";

    public TMP_Dropdown dropdown;

    IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = PlayerPrefs.GetInt(PREFS_LOCALE_ID, 0);
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.name));
        }
        dropdown.options = options;

        dropdown.value = selected;
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt(PREFS_LOCALE_ID, index);
    }
}
