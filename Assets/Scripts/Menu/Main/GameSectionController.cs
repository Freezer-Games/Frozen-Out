using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class GameSectionController : MonoBehaviour
    {
        public OptionsMenuController OptionsMenuController;
        private MainMenuManager MainMenuManager => OptionsMenuController.MainMenuManager;

        public Canvas GameSectionCanvas;

        public Dropdown LanguageDropdown;
        public Slider SizeSlider;
        public Text SizeText;

        void Start()
        {
            SizeSlider.value = MainMenuManager.GetTextSize();
            LanguageDropdown.AddOptions(
                MainMenuManager.GetSupportedLanguages()
            );
            LanguageDropdown.value = MainMenuManager.GetSupportedLanguages().IndexOf(MainMenuManager.GetLanguage());
            
            LanguageDropdown.onValueChanged.AddListener(ChangeLanguage);
            SizeSlider.onValueChanged.AddListener(ChangeTextSize);
        }

        public void Open()
        {
            GameSectionCanvas.enabled = true;
        }

        public void Close()
        {
            GameSectionCanvas.enabled = false;
        }

        private void ChangeLanguage(int languageIndex)
        {
            string language = MainMenuManager.GetSupportedLanguages()[languageIndex];
            MainMenuManager.SetLanguage(language);
        }

        private void ChangeTextSize(float newTextSize)
        {
            MainMenuManager.SetTextSize(newTextSize);
            SizeText.text = newTextSize.ToString();
        }

    }
}