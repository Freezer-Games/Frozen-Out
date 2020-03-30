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
        
        public Button ContinueButton;
        public Button SaveButton;
        public Button LoadButton;
        public Button RestartButton;
        public Button ExitButton;

        void Start()
        {
            PauseMenuCanvas.enabled = false;

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

        void Restart()
        {
            Close();

            PauseMenuManager.RestartLevel();
        }

        void SaveGame()
        {
            PauseMenuManager.SaveGame();
        }

        void LoadGame()
        {
            Close();

            //TODO
            //PauseMenuManager.LoadGame();
        }

        void Exit()
        {
            Close();

            PauseMenuManager.Exit();
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