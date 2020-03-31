using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Settings;
using Scripts.Localisation;

namespace Scripts.Menu.Pause
{
    public class PauseMenuManager : MonoBehaviour
    {

        public PauseMenuController PauseMenuController;
        public LocalisationManager LocalisationManager;

        private GameManager GameManager => GameManager.Instance;
        private SettingsManager SettingsManager => GameManager.SettingsManager;

        public bool IsEnabled
        {
            get;
            private set;
        }

        void Start()
        {
            LocalisationManager.LoadLocalisedText("Menu_pausa_Default.json");
        }

        public void Open()
        {
            PauseMenuController.Open();
        }

        public void Close()
        {
            PauseMenuController.Close();
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void Enable()
        {
            IsEnabled = true;
        }
        
        public KeyCode GetPauseKey()
        {
            return SettingsManager.PauseKey;
        }

        public void RestartLevel()
        {
            GameManager.RestartLevel();
        }

        public void SaveGame()
        {
            GameManager.SaveGame();
        }

        public void LoadGame(int loadIndex)
        {
            GameManager.LoadGame(loadIndex);
        }

        public void Exit()
        {
            GameManager.LoadMainMenu();
        }
        
    }

}