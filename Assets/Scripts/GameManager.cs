using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Scripts.Level;
using Scripts.Menu;
using Scripts.Input;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance
        {
            get
            {
                return Singleton;
            }
        }
        private static readonly GameManager Singleton = new GameManager();

        private GameManager()
        {
            DontDestroyOnLoad(gameObject);
        }

        public ILevelManager CurrentLevelManager
        {
            get;
            private set;
        }
        public MenuManager MenuManager
        {
            get;
            private set;
        }
        public InputManager InputManager
        {
            get;
            private set;
        }

        private ILevelManager[] Levels;
        private int CurrentLevel;

        public void NextLevel()
        {
            // TODO
        }

    }
}
