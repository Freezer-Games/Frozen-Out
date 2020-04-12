using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scripts.Level;
using Scripts.Menu.Pause;
using Scripts.Menu.Load;
using Scripts.Settings;
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
        private void CreateSingleton()
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
        public LoadingScreenManager LoadingScreenManager;
        public SettingsManager SettingsManager;
        public SaveManager SaveManager;
        public PlayerInfo PlayerInfo;

        private int CurrentLevelIndex = 0;
        
        private int MainMenuIndex = 1;
        private int FirstLevelIndex = 2;

        void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            ShowOnlyIntro();
            //Wait for Settings to be Ready
            SettingsManager.Ready += (sender, args) => StartGame();
        }

        private void ShowOnlyIntro()
        {
            LoadingScreenManager.ShowIntro();
            LoadingScreenManager.HideLoading();
            PauseMenuManager.Disable();
            PauseMenuManager.Close();
        }
        
        #region SettingsManager
        public void SetMusicVolume(float newVolume)
        {
            SettingsManager.SetMusicVolume(newVolume);
            AudioListener.volume = Mathf.Clamp(newVolume / 100f, 0, 1);
        }
        #endregion

        #region SaveManager
        public void SaveGame()
        {
            SaveManager.Save();
        }

        public void LoadGame(int loadIndex)
        {
            SaveManager.Load(loadIndex);
        }

        public void ContinueGame()
        {
            SaveManager.LoadLastLevel();
            // TODO
        }
        #endregion

        #region Load
        private void StartGame()
        {
            #if UNITY_EDITOR
            CurrentLevelIndex = SceneManager.GetAllScenes()[0].buildIndex; //Obtener index de la otra escena
            if(CurrentLevelIndex == 0)
            {
                LoadMainMenu();
            }
            else if(CurrentLevelIndex == MainMenuIndex)
            {
                AfterLoadMainMenu();
            }
            else if (CurrentLevelIndex >= FirstLevelIndex)
            {
                Debug.LogWarning("Reloading - Ignore previous console messages");
                Debug.ClearDeveloperConsole();
                RestartLevel(); // Reload everything to properly load GameManager before Level
            }
            LoadingScreenManager.HideIntro();
            #else
            LoadMainMenu();
            #endif
        }
        public void StartNewGame()
        {
            LoadLevel(FirstLevelIndex);
        }

        public void LoadTestLevel()
        {
            LoadLevel("Test");
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
            LoadLevel(CurrentLevelIndex);
        }

        public void LoadLevel(string levelName)
        {
            int levelIndex = SceneManager.GetSceneByName(levelName).buildIndex;
            LoadLevel(levelIndex);
        }

        public void LoadMainMenu()
        {
            BeforeLoadLevel();

            CurrentLevelIndex = MainMenuIndex;
            AsyncOperation asyncSceneLoading = SceneManager.LoadSceneAsync(CurrentLevelIndex);

            asyncSceneLoading.completed += (var) => AfterLoadMainMenu();
        }

        private void LoadLevel(int levelIndex)
        {
            BeforeLoadLevel();

            CurrentLevelIndex = levelIndex;
            AsyncOperation asyncSceneLoading = SceneManager.LoadSceneAsync(CurrentLevelIndex);

            asyncSceneLoading.completed += (var) => AfterLoadLevel();
        }

        private void BeforeLoadLevel()
        {
            if(CurrentLevelManager != null) CurrentLevelManager.Unload();
            CurrentLevelManager = null;

            LoadingScreenManager.ShowLoading();
            PauseMenuManager.Disable();
        }
        
        private void AfterLoadLevel()
        {
            LoadingScreenManager.HideLoading();
            PauseMenuManager.Enable();

            CurrentLevelManager = Object.FindObjectOfType<LevelManager>(); //Opción 1: GameManager encuentra LevelManager
            CurrentLevelManager.Load();
        }

        private void AfterLoadMainMenu()
        {
            LoadingScreenManager.HideLoading();
            LoadingScreenManager.HideIntro();
            PauseMenuManager.Disable();
        }
        #endregion

    }
}