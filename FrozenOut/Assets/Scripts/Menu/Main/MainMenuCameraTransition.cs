using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Scripts.Menu
{
    public class MainMenuCameraTransition : MonoBehaviour
    {
        public CinemachineVirtualCamera MainCamera;
        public CinemachineVirtualCamera OptionsCamera;

        private int MainCameraPriority;

        void Start()
        {
            MainCameraPriority = MainCamera.Priority;
        }

        public void GoToOptions()
        {
            MainCamera.Priority = 0;
        }

        public void GoToMain()
        {
            MainCamera.Priority = MainCameraPriority;
        }

        /*private IEnumerator DoGoToMain()
        {
            MiddleCamera.Priority = MiddleCameraPriority;

            yield return new WaitForSeconds(TransitionDelay);

            MainCamera.Priority = MainCameraPriority;

            yield return new WaitForSeconds(TransitionDelay);
        }

        private IEnumerator DoGoToOptions()
        {
            MainCamera.Priority = 0;

            yield return new WaitForSeconds(TransitionDelay);

            MiddleCamera.Priority = 0;

            yield return new WaitForSeconds(TransitionDelay);
        }*/
    }
}