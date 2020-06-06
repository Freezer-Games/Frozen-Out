using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class MainMenuController : UIController
    {
        public MainMenuManager MainMenuManager;

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

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
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
            //TODO
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