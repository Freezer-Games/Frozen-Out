using UnityEngine;
using System.Collections;
using Scripts.Level;

namespace Scripts.Menu.GameOver
{
    public class TriggerGameOver : TriggerBase
    {
        private ILevelManager LevelManager => GameManager.CurrentLevelManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                LevelManager.GameOver();
            }
        }
    }
}