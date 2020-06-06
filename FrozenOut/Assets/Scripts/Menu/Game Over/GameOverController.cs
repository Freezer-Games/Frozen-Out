using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.GameOver
{
    public class GameOverController : UIController
    {
        public GameOverManager GameOverManager;

        public Button RestartButton;
        public Button MainLevelButton;

        private void Start()
        {
            RestartButton.onClick.AddListener(RestartLevel);
            MainLevelButton.onClick.AddListener(MainLevel);
        }

        private void RestartLevel()
        {
            GameOverManager.RestartLevel();
        }

        private void MainLevel()
        {
            GameOverManager.MainLevel();
        }
    }
}