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
        private bool IsOpen => PauseMenuCanvas.enabled;
        
        public Button ContinueButton;
        public Button SaveButton;
        public Button LoadButton;
        public Button RestartButton;
        public Button ExitButton;

        void Start()
        {
            ContinueButton.onClick.AddListener(CloseOpenMenu);
            SaveButton.onClick.AddListener(SaveGame);
            LoadButton.onClick.AddListener(LoadGame);
            RestartButton.onClick.AddListener(Restart);
            ExitButton.onClick.AddListener(Exit);
        }

        void Update()
        {
            if (Input.GetKeyDown(PauseMenuManager.GetPauseKey()))
            {
                CloseOpenMenu();
            }
        }

        private void Restart()
        {
            Close();

            PauseMenuManager.RestartLevel();
        }

        private void SaveGame()
        {
            PauseMenuManager.SaveGame();
        }

        private void LoadGame()
        {
            Close();

            //TODO
            //PauseMenuManager.LoadGame();
        }

        private void Exit()
        {
            Close();

            PauseMenuManager.Exit();
        }

        public void Open()
        {
            PauseMenuCanvas.enabled = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Close()
        {
            PauseMenuCanvas.enabled = false;
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void CloseOpenMenu()
        {
            if(PauseMenuManager.IsEnabled)
            {
                if(IsOpen)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }

    }
}