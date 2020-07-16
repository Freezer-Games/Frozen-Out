using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class GraphicsSectionController : UIController
    {
        public OptionsMenuController OptionsMenuController;
        private MainMenuManager MainMenuManager => OptionsMenuController.MainMenuManager;

        public Dropdown ResolutionDropdown;
        public Dropdown AspectRatioDropdown;
        public Dropdown ScreenTypeDropdown;
        public Dropdown QualityDropdown;
        /*public Button LowResButton;
        public Button MediumResButton;
        public Button HighResButton;*/
        public Button ApplyButton;

        void Start()
        {
            ResolutionDropdown.AddOptions(
                MainMenuManager.GetSupportedResolutions()
            );
            ResolutionDropdown.value = MainMenuManager.GetSupportedResolutions().IndexOf(MainMenuManager.GetResolution());

            AspectRatioDropdown.AddOptions(
                MainMenuManager.GetSupportedAspectRatios()
            );
            AspectRatioDropdown.value = MainMenuManager.GetSupportedAspectRatios().IndexOf(MainMenuManager.GetAspectRatio());

            ScreenTypeDropdown.AddOptions(
                MainMenuManager.GetSupportedScreenTypes()
            );
            ScreenTypeDropdown.value = MainMenuManager.GetSupportedScreenTypes().IndexOf(MainMenuManager.GetScreenType());

            QualityDropdown.AddOptions(new List<string>()
                {
                    "Low",
                    "Medium",
                    "High"
                }
            );
            QualityDropdown.value = 0;

            ApplyButton.onClick.AddListener(ApplySettings);
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        private void ApplySettings()
        {
            int resolutionIndex = ResolutionDropdown.value;
            int aspectRatioIndex = AspectRatioDropdown.value;
            int screenTypeIndex = ScreenTypeDropdown.value;
            string aspectRatio = MainMenuManager.GetSupportedAspectRatios()[aspectRatioIndex];
            string resolution = MainMenuManager.GetSupportedResolutions(aspectRatio)[resolutionIndex];
            string screenType = MainMenuManager.GetSupportedScreenTypes()[screenTypeIndex];
            
            MainMenuManager.SetAspectRatio(aspectRatio);
            MainMenuManager.SetResolution(resolution, screenTypeIndex == 0);
            MainMenuManager.SetScreenType(screenType);

            int qualityIndex = QualityDropdown.value;
            switch (qualityIndex)
            {
                case 0:
                    MainMenuManager.SetLowQuality();
                    break;
                case 1:
                    MainMenuManager.SetMediumQuality();
                    break;
                case 2:
                    MainMenuManager.SetHighQuality();
                    break;
            }
        }

    }
}