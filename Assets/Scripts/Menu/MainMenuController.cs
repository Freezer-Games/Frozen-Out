using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class MainMenuController : MonoBehaviour
    {
        
        public MainMenuManager MainMenuManager;

        public Button ExitButton;

        private GameManager GameManager;

        void Start()
        {
            GameManager = GameManager.Instance;

            ExitButton.onClick.AddListener(Exit);
        }

        void Exit()
        {
            GameManager.Quit();
        }

    }
}