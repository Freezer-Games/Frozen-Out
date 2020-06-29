using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Scripts.Menu.Main
{
    public class MainMenuCameraTransition : MonoBehaviour
    {
        public CinemachineVirtualCamera MainCamera;
        public CinemachineVirtualCamera OptionsCamera;
        public GameObject ScreenCamera;

        private int MainCameraPriority;

        void Start()
        {
            MainCameraPriority = MainCamera.Priority;
        }

        public void GoToOptions()
        {
            MainCamera.Priority = 0;
            ScreenCamera.SetActive(false);
        }

        public void GoToMain()
        {
            MainCamera.Priority = MainCameraPriority;
            StartCoroutine(ActiveScreenCamera());
        }

        private IEnumerator ActiveScreenCamera()
        {
            yield return new WaitForSeconds(1f);
            ScreenCamera.SetActive(true);
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