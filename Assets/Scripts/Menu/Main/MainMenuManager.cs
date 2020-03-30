using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Settings;

using Scripts.Localisation;

namespace Scripts.Menu.Main
{
    public class MainMenuManager : MonoBehaviour
    {
        
        public MainMenuController MainMenuController;
        public OptionsMenuController OptionsMenuController;
        public LocalisationManager LocalisationManager;

        private GameManager GameManager => GameManager.Instance;
        private SettingsManager SettingsManager => GameManager.SettingsManager;

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
            return SettingsManager.MusicVolume;
        }

        public void SetMusicVolume(float newVolume)
        {
            GameManager.SetMusicVolume(newVolume);
        }

        public float GetTextSize()
        {
            return SettingsManager.TextSize;
        }

        public void SetTextSize(float newTextSize)
        {
            SettingsManager.SetTextSize(newTextSize);
        }

        public string GetLanguage()
        {
            return SettingsManager.Language;
        }

        public void SetLanguage(string newLanguage)
        {
            SettingsManager.SetLanguage(newLanguage);
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
            return SettingsManager.SupportedLanguages;
        }

        public string GetAspectRatio()
        {
            return SettingsManager.AspectRatio;
        }

        public void SetAspectRatio(string newAspectRatio)
        {
            SettingsManager.SetAspectRatio(newAspectRatio);
        }

        public List<string> GetSupportedAspectRatios()
        {
            return SettingsManager.SupportedAspectRatios;
        }

        public string GetScreenType()
        {
            return SettingsManager.ScreenType;
        }

        public void SetScreenType(string newScreenType)
        {
            SettingsManager.SetScreenType(newScreenType);
        }

        public List<string> GetSupportedScreenTypes()
        {
            return SettingsManager.SupportedScreenTypes;
        }

        public string GetResolution()
        {
            return SettingsManager.Resolution;
        }

        public void SetResolution(string newResolution, bool isFullscreen)
        {
            char[] separator = { 'x' };
            string[] strlist = newResolution.Split(separator);
            int resWidth = Int32.Parse(strlist[0]);
            int resHeight = Int32.Parse(strlist[1]);

            Screen.SetResolution(resWidth, resHeight, isFullscreen);
            SettingsManager.SetResolution(newResolution);
        }

        public List<string> GetSupportedResolutions()
        {
            string aspectRatio = GetAspectRatio();
            return GetSupportedResolutions(aspectRatio);
        }

        public List<string> GetSupportedResolutions(string aspectRatio)
        {
            return SettingsManager.SupportedResolutions[aspectRatio];
        }

        public string GetQuality()
        {
            return SettingsManager.Quality;
        }

        public void SetLowQuality()
        {
            QualitySettings.masterTextureLimit = 8;
            SettingsManager.SetLowQuality();
        }

        public void SetMediumQuality()
        {
            QualitySettings.masterTextureLimit = 4;
            SettingsManager.SetMediumQuality();
        }

        public void SetHighQuality()
        {
            QualitySettings.masterTextureLimit = 0;
            SettingsManager.SetHighQuality();
        }

        public KeyCode GetForwardKey()
        {
            return SettingsManager.ForwardKey;
        }

        public void SetForwardKey(KeyCode keyCode)
        {
            SettingsManager.SetForwardKey(keyCode);
        }

        public KeyCode GetBackKey()
        {
            return SettingsManager.BackwardKey;
        }

        public void SetBackKey(KeyCode keyCode)
        {
            SettingsManager.SetBackwardKey(keyCode);
        }

        public KeyCode GetRightKey()
        {
            return SettingsManager.RightKey;
        }

        public void SetRightKey(KeyCode keyCode)
        {
            SettingsManager.SetRightKey(keyCode);
        }

        public KeyCode GetLeftKey()
        {
            return SettingsManager.LeftKey;
        }

        public void SetLeftKey(KeyCode keyCode)
        {
            SettingsManager.SetLeftKey(keyCode);
        }

        public KeyCode GetJumpKey()
        {
            return SettingsManager.JumpKey;
        }

        public void SetJumpKey(KeyCode keyCode)
        {
            SettingsManager.SetJumpKey(keyCode);
        }

        public KeyCode GetCrouchKey()
        {
            return SettingsManager.CrouchKey;
        }

        public void SetCrouchKey(KeyCode keyCode)
        {
            SettingsManager.SetCrouchKey(keyCode);
        }

        public KeyCode GetInteractKey()
        {
            return SettingsManager.InteractKey;
        }

        public void SetInteractKey(KeyCode keyCode)
        {
            SettingsManager.SetInteractKey(keyCode);
        }

        public KeyCode GetMissionsKey()
        {
            return SettingsManager.MissionsKey;
        }

        public void SetMissionsKey(KeyCode keyCode)
        {
            SettingsManager.SetMissionsKey(keyCode);
        }

        public KeyCode GetNextDialogueKey()
        {
            return SettingsManager.NextDialogueKey;
        }

        public void SetNextDialogueKey(KeyCode keyCode)
        {
            SettingsManager.SetNextDialogueKey(keyCode);
        }

    }
}