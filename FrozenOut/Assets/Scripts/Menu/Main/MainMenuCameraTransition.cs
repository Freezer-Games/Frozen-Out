using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Scripts.Menu.Main
{
    public class MainMenuCameraTransition : MonoBehaviour
    {
        //public CinemachineVirtualCamera MainCamera;
        //public CinemachineVirtualCamera MiddleCamera;
        //public CinemachineVirtualCamera OptionsCamera;
        public CinemachineVirtualCamera CamaraRail;

        public GameObject ScreenCamera;
        public GameObject WhiteOverlay;
        public CinemachineTrackedDolly Dolly;

        private int MainCameraPriority;
        private int MiddleCameraPriority;
        private const float TransitionDelay = 1.0f;

        void Start()
        {
            Dolly = CamaraRail.GetCinemachineComponent<CinemachineTrackedDolly>();
            //MainCameraPriority = MainCamera.Priority;
            //MiddleCameraPriority = MiddleCamera.Priority;
            StartCoroutine(ActiveMenu(true));
        }

        public void GoToOptions()
        {
            ScreenCamera.SetActive(false);
            WhiteOverlay.SetActive(false);
            Dolly.m_PathPosition = 2;
            //StartCoroutine(DoGoToOptions());
        }

        public void GoToMain()
        {
            StartCoroutine(ActiveMenu(false));

            //StartCoroutine(DoGoToMain());
        }

        private IEnumerator ActiveMenu(bool start)
        {
            if (!start)
            {
                yield return new WaitForSeconds(0.9f);
            }
            Dolly.m_PathPosition = 0;
            WhiteOverlay.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            ScreenCamera.SetActive(true);
           
        }

        private IEnumerator DoGoToMain()
        {
            //MiddleCamera.Priority = MiddleCameraPriority;

            yield return new WaitForSeconds(TransitionDelay);

            //MainCamera.Priority = MainCameraPriority;

            yield return new WaitForSeconds(TransitionDelay);
        }

        private IEnumerator DoGoToOptions()
        {
            //MainCamera.Priority = 0;

            yield return new WaitForSeconds(TransitionDelay);

            //MiddleCamera.Priority = 0;

            yield return new WaitForSeconds(TransitionDelay);
        }
    }
}