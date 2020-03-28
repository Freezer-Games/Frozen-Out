using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Localisation;
using UnityEngine.UI;
using System;

namespace Scripts.Menu.Main
{
    public class MainMenuManager : MonoBehaviour
    {
        
        public MainMenuController MainMenuController;
        public OptionsMenuController OptionsMenuController;
        public LocalisationManager LocalisationManager;

        private GameManager GameManager => GameManager.Instance;

        void Start()
        {
            MainMenuController.Open();
            OptionsMenuController.Close();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            LocalisationManager.LoadLocalisedText("Menu_Default.json");
        }
        
        public void ContinueGame()
        {
            GameManager.ContinueGame();
        }

        public void StartGame()
        {
            GameManager.StartGame();
        }

        public void Quit()
        {
            GameManager.Quit();
        }

        public void LoadTestLevel()
        {
            GameManager.LoadTestLevel();
        }

        public void OpenOptionsMenu()
        {
            OptionsMenuController.Open();
        }

        public void OpenMainMenu()
        {
            MainMenuController.Open();
        }

        public float GetMusicVolume()
        {
            return GameManager.SettingsManager.MusicVolume;
        }

        public void SetMusicVolume(float newVolume)
        {
            GameManager.SetMusicVolume(newVolume);
        }

        public float GetTextSize()
        {
            return GameManager.SettingsManager.TextSize;
        }

        public void SetTextSize(float newTextSize)
        {
            GameManager.SettingsManager.SetTextSize(newTextSize);
        }

        public string GetLanguage()
        {
            return GameManager.SettingsManager.Language;
        }

        public void SetLanguage(string newLanguage)
        {
            GameManager.SettingsManager.SetLanguage(newLanguage);
            /*LocalisationManager.SetLanguage(newLanguage);
            

        //TODO

    void ChangeLanguageF(int selection)
    {
        print(Application.streamingAssetsPath + "Menu_Default.json");
        if (selection == 0) {
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_En.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Trial_level_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Trial_level_En.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_pausa_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_pausa_En.json"));
            PlayerPrefs.SetString("Language", "En");
        }else if (selection == 1) {
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_Es.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Trial_level_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Trial_level_Es.json"));
            File.WriteAllBytes(Application.streamingAssetsPath + "/Menu_pausa_Default.json", File.ReadAllBytes(Application.streamingAssetsPath + "/Menu_pausa_Es.json"));
            PlayerPrefs.SetString("Language", "Es");
        }
        LocalizationManager.instance.LoadLocalizedText("Menu_Default.json");
    }
            */
        }

        public List<string> GetSupportedLanguages()
        {
            return GameManager.SettingsManager.SupportedLanguages;
        }

        public string GetAspectRatio()
        {
            return GameManager.SettingsManager.AspectRatio;
        }

        public void SetAspectRatio(string newAspectRatio)
        {
            GameManager.SettingsManager.SetAspectRatio(newAspectRatio);
        }

        public List<string> GetSupportedAspectRatios()
        {
            return GameManager.SettingsManager.SupportedAspectRatios;
        }

        public string GetScreenType()
        {
            return GameManager.SettingsManager.ScreenType;
        }

        public void SetScreenType(string newScreenType)
        {
            GameManager.SettingsManager.SetScreenType(newScreenType);
        }

        public List<string> GetSupportedScreenTypes()
        {
            return GameManager.SettingsManager.SupportedScreenTypes;
        }

        public string GetResolution()
        {
            return GameManager.SettingsManager.Resolution;
        }

        public void SetResolution(string newResolution, bool isFullscreen)
        {
            char[] separator = { 'x' };
            string[] strlist = newResolution.Split(separator);
            int resWidth = Int32.Parse(strlist[0]);
            int resHeight = Int32.Parse(strlist[1]);

            Screen.SetResolution(resWidth, resHeight, isFullscreen);
            GameManager.SettingsManager.SetResolution(newResolution);
        }

        public List<string> GetSupportedResolutions()
        {
            string aspectRatio = GetAspectRatio();
            return GetSupportedResolutions(aspectRatio);
        }

        public List<string> GetSupportedResolutions(string aspectRatio)
        {
            return GameManager.SettingsManager.SupportedResolutions[aspectRatio];
        }

        public string GetQuality()
        {
            return GameManager.SettingsManager.Quality;
        }

        public void SetLowQuality()
        {
            QualitySettings.masterTextureLimit = 8;
            GameManager.SettingsManager.SetLowQuality();
        }

        public void SetMediumQuality()
        {
            QualitySettings.masterTextureLimit = 4;
            GameManager.SettingsManager.SetMediumQuality();
        }

        public void SetHighQuality()
        {
            QualitySettings.masterTextureLimit = 0;
            GameManager.SettingsManager.SetHighQuality();
        }

        public KeyCode GetForwardKey()
        {
            return GameManager.SettingsManager.ForwardKey;
        }

        public void SetForwardKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetForwardKey(keyCode);
        }

        public KeyCode GetBackKey()
        {
            return GameManager.SettingsManager.BackwardKey;
        }

        public void SetBackKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetBackwardKey(keyCode);
        }

        public KeyCode GetRightKey()
        {
            return GameManager.SettingsManager.RightKey;
        }

        public void SetRightKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetRightKey(keyCode);
        }

        public KeyCode GetLeftKey()
        {
            return GameManager.SettingsManager.LeftKey;
        }

        public void SetLeftKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetLeftKey(keyCode);
        }

        public KeyCode GetJumpKey()
        {
            return GameManager.SettingsManager.JumpKey;
        }

        public void SetJumpKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetJumpKey(keyCode);
        }

        public KeyCode GetCrouchKey()
        {
            return GameManager.SettingsManager.CrouchKey;
        }

        public void SetCrouchKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetCrouchKey(keyCode);
        }

        public KeyCode GetInteractKey()
        {
            return GameManager.SettingsManager.InteractKey;
        }

        public void SetInteractKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetInteractKey(keyCode);
        }

        public KeyCode GetMissionsKey()
        {
            return GameManager.SettingsManager.MissionsKey;
        }

        public void SetMissionsKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetMissionsKey(keyCode);
        }

        public KeyCode GetNextDialogueKey()
        {
            return GameManager.SettingsManager.NextDialogueKey;
        }

        public void SetNextDialogueKey(KeyCode keyCode)
        {
            GameManager.SettingsManager.SetNextDialogueKey(keyCode);
        }

    }
}