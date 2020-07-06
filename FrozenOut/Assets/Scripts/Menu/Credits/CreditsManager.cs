using UnityEngine;

namespace Scripts.Menu.Credits
{
    public class CreditsManager : MonoBehaviour
    {
        private GameManager GameManager => GameManager.Instance;

        public void FinishCredits()
        {
            GameManager.LoadMainMenu();
        }
    }
}