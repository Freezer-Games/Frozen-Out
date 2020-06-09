using UnityEngine;

namespace Scripts
{
    public abstract class BaseManager : MonoBehaviour
    {
        protected GameManager GameManager => GameManager.Instance;

        private bool ManagerEnabled
        {
            get;
            set;
        }

        public void Enable()
        {
            ManagerEnabled = true;
        }
        public void Disable()
        {
            ManagerEnabled = false;
        }

        public bool IsEnabled()
        {
            return ManagerEnabled;
        }
    }
}