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

        public void Open()
        {
            GameOverController.Open();
        }

        public void Close()
        {
            GameOverController.Close();
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