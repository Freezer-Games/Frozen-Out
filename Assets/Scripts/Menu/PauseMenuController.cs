using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Pause
{
    public class PauseMenuController : MonoBehaviour
    {

        public PauseMenuManager PauseMenuManager;

        public Canvas PauseMenuCanvas;
        public Canvas LoadCanvas;
        
        public Button ContinueButton;
        public Button SaveButton;
        public Button LoadButton;
        public Button RestartButton;
        public Button ExitButton;

        private GameManager GameManager;

        void Start()
        {
            GameManager = GameManager.Instance;
            PauseMenuCanvas.enabled = false;
            LoadCanvas.enabled = false;

            ContinueButton.onClick.AddListener(CloseOpenMenu);
            SaveButton.onClick.AddListener(SaveGame);
            LoadButton.onClick.AddListener(LoadGame);
            RestartButton.onClick.AddListener(Restart);
            ExitButton.onClick.AddListener(Exit);
        }

        void Update()
        {
            if (Input.GetKeyDown(GameManager.SettingsManager.PauseKey))
            {
                CloseOpenMenu();
            }
        }

        void Restart()
        {
            Close();

            GameManager.RestartLevel();
        }

        void SaveGame()
        {
            GameManager.SaveGame();
        }

        void LoadGame()
        {
            Close();

            GameManager.LoadGame();
        }

        void Exit()
        {
            Close();

            GameManager.LoadMainMenu();
        }

        private void Close()
        {
            PauseMenuManager.Close();
            PauseMenuCanvas.enabled = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Open()
        {
            PauseMenuManager.Open();
            PauseMenuCanvas.enabled = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        void CloseOpenMenu()
        {
            
            if (PauseMenuManager.IsEnabled && !PauseMenuManager.IsOpen)
            {
                Open();
            }
            else if (PauseMenuManager.IsEnabled && PauseMenuManager.IsOpen)
            {
                Close();
            }


        }

    }
}