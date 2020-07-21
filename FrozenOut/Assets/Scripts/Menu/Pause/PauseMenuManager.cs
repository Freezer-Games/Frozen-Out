using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Settings;

namespace Scripts.Menu.Pause
{
    public class PauseMenuManager : BaseManager
    {

        public UIController PauseMenuController;

        private SettingsManager SettingsManager => GameManager.SettingsManager;

        public void Open()
        {
            if (IsEnabled())
            {
                PauseMenuController.Open();
            }
        }

        public void Close()
        {
            PauseMenuController.Close();
        }

        public void EnableLevel()
        {
            GameManager.EnableLevel();
        }

        public void DisableLevel()
        {
            GameManager.DisableLevel();
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