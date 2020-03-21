using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scripts.Level;
using Scripts.Menu;
using Scripts.Settings;
using Scripts.Localisation;

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
                DontDestroyOnLoad(gameObject);
            }
        }
        #endregion
        
        public ILevelManager CurrentLevelManager
        {
            get;
            private set;
        }
        public MenuManager MenuManager;
        public SettingsManager SettingsManager;
        public LocalisationManager LocalisationManager;
        public LocalisationManager MenuLocalisationManager;

        private int CurrentLevel = 0;

        void Awake()
        {
            CheckSingleton();
        }

        void Start()
        {
            MenuManager.Close();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Time.timeScale = 1;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            
            if (scene.buildIndex == 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                LocalisationManager.LoadLocalisedText("Menu_Default.json");

            } else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                LocalisationManager.LoadLocalisedText("Trial_level_Default.json");
                MenuLocalisationManager.LoadLocalisedText("Menu_pausa_Default.json");
            }

        }

        public void LoadNextLevel()
        {
            LoadLevel(CurrentLevel + 1);
        }

        public void LoadLevel(int level)
        {
            CurrentLevelManager.Unload();

            CurrentLevel = level;
            SceneManager.LoadScene(CurrentLevel);

            CurrentLevelManager = Object.FindObjectOfType<LevelManager>();
            CurrentLevelManager.Load();
        }

        public void CinematicMode()
        {
            CurrentLevelManager.GetPlayerManager().ToCinematic();
            
            CurrentLevelManager.GetCameraManager().ToCinematic();
        }

        public void NormalMode()
        {
            CurrentLevelManager.GetPlayerManager().ToNormal();

            CurrentLevelManager.GetCameraManager().ToNormal();
        }

    }
}

