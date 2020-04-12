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
        public Button LowResButton;
        public Button MediumResButton;
        public Button HighResButton;
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

            AspectRatioDropdown.onValueChanged.AddListener(UpdateResolutions);
            LowResButton.onClick.AddListener(LowResAction);
            MediumResButton.onClick.AddListener(MediumResAction);
            HighResButton.onClick.AddListener(HighResAction);
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

        private void UpdateResolutions(int aspectRatioIndex)
        {
            string aspectRatio = MainMenuManager.GetSupportedAspectRatios()[aspectRatioIndex];
            ResolutionDropdown.ClearOptions();
            ResolutionDropdown.AddOptions(
                MainMenuManager.GetSupportedResolutions(aspectRatio)
            );
            ResolutionDropdown.value = 0;
            ResolutionDropdown.RefreshShownValue();
        }

        private void LowResAction()
        {
            LowResButton.interactable = false;
            MediumResButton.interactable = true;
            HighResButton.interactable = true;
        }

        private void MediumResAction()
        {
            LowResButton.interactable = true;
            MediumResButton.interactable = false;
            HighResButton.interactable = true;
        }

        private void HighResAction()
        {
            LowResButton.interactable = true;
            MediumResButton.interactable = true;
            HighResButton.interactable = false;
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

            if(!LowResButton.IsInteractable())
            {
                MainMenuManager.SetLowQuality();
            }
            else if(!MediumResButton.IsInteractable())
            {
                MainMenuManager.SetMediumQuality();
            }
            else if(!HighResButton.IsInteractable())
            {
                MainMenuManager.SetHighQuality();
            }
        }

    }
}