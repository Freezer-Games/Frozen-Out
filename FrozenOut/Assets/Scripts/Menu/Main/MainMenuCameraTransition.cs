using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Scripts.Menu.Main
{
    public class MainMenuCameraTransition : MonoBehaviour
    {
        public CinemachineVirtualCamera MainCamera;
        public CinemachineVirtualCamera MiddleCamera;
        public CinemachineVirtualCamera OptionsCamera;

        public GameObject ScreenCamera;
        public GameObject WhiteOverlay;

        private int MainCameraPriority;
        private int MiddleCameraPriority;
        private const float TransitionDelay = 1.0f;

        void Start()
        {
            MainCameraPriority = MainCamera.Priority;
            MiddleCameraPriority = MiddleCamera.Priority;
            StartCoroutine(ActiveMenu(true));
        }

        public void GoToOptions()
        {
            ScreenCamera.SetActive(false);
            WhiteOverlay.SetActive(false);

            StartCoroutine(DoGoToOptions());
        }

        public void GoToMain()
        {
            StartCoroutine(ActiveMenu(false));

            StartCoroutine(DoGoToMain());
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

        private IEnumerator DoGoToMain()
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
        }
    }
}