using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class AudioSectionController : MonoBehaviour
    {
        public OptionsMenuController OptionsMenuController;
        private MainMenuManager MainMenuManager => OptionsMenuController.MainMenuManager;

        public Canvas AudioSectionCanvas;

        public Slider AudioSlider;
        public Text VolumeText;

        void Start()
        {
            AudioSlider.value = MainMenuManager.GetMusicVolume();

            AudioSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        public void Open()
        {
            AudioSectionCanvas.enabled = true;
        }

        public void Close()
        {
            AudioSectionCanvas.enabled = false;
        }

        private void ChangeMusicVolume(float newVolume)
        {
            MainMenuManager.SetMusicVolume(newVolume);
            VolumeText.text = newVolume.ToString();
        }

    }
}