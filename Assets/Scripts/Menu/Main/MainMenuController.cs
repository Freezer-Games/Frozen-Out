using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class MainMenuController : MonoBehaviour
    {
        
        public MainMenuManager MainMenuManager;

        public SelectLoadController SelectLoadController;

        public Canvas MainCanvas;

        public Button StartButton;
        public Button ContinueButton;
        public Button LoadButton;
        public Button OptionsButton;
        public Button ExitButton;
        public Button TestButton;

        void Start()
        {
            StartButton.onClick.AddListener(StartGame);
            ContinueButton.onClick.AddListener(ContinueGame);
            LoadButton.onClick.AddListener(OpenSelectLoad);
            OptionsButton.onClick.AddListener(OpenOptionsMenu);
            ExitButton.onClick.AddListener(Exit);
            TestButton.onClick.AddListener(LoadTestLevel);
        }

        public void Open()
        {
            MainCanvas.enabled = true;
            SelectLoadController.Close();
        }

        public void Close()
        {
            MainCanvas.enabled = false;
        }

        private void LoadTestLevel()
        {
            MainMenuManager.LoadTestLevel();
        }

        private void Exit()
        {
            MainMenuManager.Quit();
        }

        private void StartGame()
        {
            MainMenuManager.StartGame();
        }

        private void OpenSelectLoad()
        {
            SelectLoadController.Open();
        }

        private void ContinueGame()
        {
            MainMenuManager.ContinueGame();
        }

        private void OpenOptionsMenu()
        {
            Close();
            MainMenuManager.OpenOptionsMenu();
        }

    }
}