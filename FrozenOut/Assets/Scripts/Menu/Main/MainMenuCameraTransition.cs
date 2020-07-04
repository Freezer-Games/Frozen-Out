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
        public GameObject WhiteOverlay;

        private int MainCameraPriority;

        void Start()
        {
            MainCameraPriority = MainCamera.Priority;
            StartCoroutine(ActiveMenu(true));
        }

        public void GoToOptions()
        {
            MainCamera.Priority = 0;
            ScreenCamera.SetActive(false);
            WhiteOverlay.SetActive(false);
        }

        public void GoToMain()
        {
            MainCamera.Priority = MainCameraPriority;
            StartCoroutine(ActiveMenu(false));
        }

        private IEnumerator ActiveMenu(bool start)
        {
            if (!start)
            {
                yield return new WaitForSeconds(0.9f);
            }
            WhiteOverlay.SetActive(true);
            yield return new WaitForSeconds(0.1f);
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