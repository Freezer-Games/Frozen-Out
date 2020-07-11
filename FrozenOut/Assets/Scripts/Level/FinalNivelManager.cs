using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level
{
    public class FinalNivelManager : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;

        public void LoadNextLevel()
        {
            GameManager.LoadNextLevel();
        }
    }
}