using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Scripts.Menu.GameOver
{
    public class GameOverController : UIController
    {
        public GameOverManager GameOverManager;

        public Button RestartButton;
        public Button MainLevelButton;
        public PlayableDirector GameOverDirector;

        private void Start()
        {
            RestartButton.onClick.AddListener(RestartLevel);
            MainLevelButton.onClick.AddListener(MainLevel);
        }

        public override void Open()
        {
            base.Open();

            GameOverDirector.Play();
        }

        private void RestartLevel()
        {
            Close();

            GameOverManager.RestartLevel();
        }

        private void MainLevel()
        {
            Close();

            GameOverManager.MainLevel();
        }
    }
}