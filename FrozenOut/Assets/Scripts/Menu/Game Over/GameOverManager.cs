using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Menu.GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        public UIController GameOverController;

        private GameManager GameManager => GameManager.Instance;

        public void ShowGameOver()
        {
            GameOverController.Open();
        }

        public void RestartLevel()
        {
            GameManager.RestartLevel();
        }

        public void MainLevel()
        {
            GameManager.LoadMainMenu();
        }
    }
}