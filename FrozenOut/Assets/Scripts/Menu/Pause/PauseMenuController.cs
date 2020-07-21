using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Pause
{
    public class PauseMenuController : UIController
    {
        public PauseMenuManager PauseMenuManager;
        
        public Button ContinueButton;
        public Button RestartButton;
        public Button ExitButton;

        void Start()
        {
            ContinueButton.onClick.AddListener(CloseOpenMenu);
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

        private void Exit()
        {
            Close();

            PauseMenuManager.Exit();
        }

        public override void Open()
        {
            base.Open();
            PauseMenuManager.DisableLevel();

            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public override void Close()
        {
            base.Close();
            PauseMenuManager.EnableLevel();

            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void CloseOpenMenu()
        {
            if(PauseMenuManager.IsEnabled())
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