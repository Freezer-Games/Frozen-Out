using UnityEngine;
using Cinemachine;
using System.Collections;

namespace Scripts.Menu.Main
{
    public class MainMenuCameraTransition : MonoBehaviour
    {
        public CinemachineVirtualCamera CamaraRail;

        public GameObject ScreenCamera;
        public GameObject WhiteOverlay;

        private CinemachineTrackedDolly Dolly;

        void Start()
        {
            Dolly = CamaraRail.GetCinemachineComponent<CinemachineTrackedDolly>();
            StartCoroutine(ActiveMenu(true));
        }

        public void GoToOptions()
        {
            ScreenCamera.SetActive(false);
            WhiteOverlay.SetActive(false);
            Dolly.m_PathPosition = 2;
        }

        public void GoToMain()
        {
            StartCoroutine(ActiveMenu(false));
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
    }
}