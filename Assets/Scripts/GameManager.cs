using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scripts.Level;
using Scripts.Menu.Pause;
using Scripts.Settings;
using Scripts.Localisation;
using Scripts.Player;
using Scripts.Save;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {

        #region Singleton
        public static GameManager Instance
        {
            get
            {
                return Singleton;
            }
        }
        private static GameManager Singleton;
        private void CheckSingleton()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(this.gameObject);
            } else {
                Singleton = this;
            }
        }
        #endregion
        
        public ILevelManager CurrentLevelManager
        {
            get;
            private set;
        }
        public PauseMenuManager PauseMenuManager;
        public SettingsManager SettingsManager;
        public SaveManager SaveManager;
        public PlayerInformation PlayerInformation;
        public LocalisationManager LocalisationManager;

        private int CurrentLevelIndex = 0;

        void Awake()
        {
            CheckSingleton();

            #if UNITY_EDITOR
            CurrentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            CurrentLevelManager = Object.FindObjectOfType<LevelManager>();
            #endif
        }

        void Start()
        {
            PauseMenuManager.Close();
            Time.timeScale = 1;
        }

        public void SaveGame()
        {
            SaveManager.Save();
        }

        public void LoadGame()
        {
            SaveManager.Load();
        }

        public void Quit()
        {
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }

        public void LoadNextLevel()
        {
            LoadLevel(CurrentLevelIndex + 1);
        }

        public void RestartLevel()
        {
            LoadLevel(CurrentLevelIndex); //TODO sin volver a buscar el LevelManager se debería poder reiniciar
        }

        public void LoadMainMenu()
        {
            CurrentLevelManager.Unload();
            CurrentLevelManager = null;

            CurrentLevelIndex = 0;
            SceneManager.LoadScene(0, LoadSceneMode.Single);

            PauseMenuManager.Disable();
        }

        public void LoadLevel(string levelName)
        {
            int levelIndex = SceneManager.GetSceneByName(levelName).buildIndex;
            LoadLevel(levelIndex);
        }

        private void LoadLevel(int levelIndex)
        {
            CurrentLevelManager.Unload();

            CurrentLevelIndex = levelIndex;
            SceneManager.LoadScene(CurrentLevelIndex);

            PauseMenuManager.Enable();

            CurrentLevelManager = Object.FindObjectOfType<LevelManager>(); //Opción 1: GameManager encuentra LevelManager
            CurrentLevelManager.Load();
        }

    }
}

