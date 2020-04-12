using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class GameSectionController : UIController
    {
        public OptionsMenuController OptionsMenuController;
        private MainMenuManager MainMenuManager => OptionsMenuController.MainMenuManager;

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

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        private void ChangeLanguage(int languageIndex)
        {
            MainMenuManager.SetLanguage(languageIndex);
        }

        private void ChangeTextSize(float newTextSize)
        {
            MainMenuManager.SetTextSize(newTextSize);
            SizeText.text = newTextSize.ToString();
        }

    }
}