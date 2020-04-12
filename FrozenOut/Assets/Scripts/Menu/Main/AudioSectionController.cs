using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class AudioSectionController : UIController
    {
        public OptionsMenuController OptionsMenuController;
        private MainMenuManager MainMenuManager => OptionsMenuController.MainMenuManager;

        public Slider AudioSlider;
        public Text VolumeText;

        void Start()
        {
            AudioSlider.value = MainMenuManager.GetMusicVolume();

            AudioSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        private void ChangeMusicVolume(float newVolume)
        {
            MainMenuManager.SetMusicVolume(newVolume);
            VolumeText.text = newVolume.ToString();
        }

    }
}