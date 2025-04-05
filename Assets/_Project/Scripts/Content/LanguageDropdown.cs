using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

namespace Project.Content
{
    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _languageDropdown;

        private void Start()
        {
            _languageDropdown.ClearOptions();

            List<string> languageOptions = new List<string>();

            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                languageOptions.Add(locale.LocaleName);
            }

            _languageDropdown.AddOptions(languageOptions);

            int currentIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
            _languageDropdown.value = currentIndex;
            _languageDropdown.RefreshShownValue();

            _languageDropdown.onValueChanged.AddListener(SetLanguage);
        }

        public void SetLanguage(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
    }
}
