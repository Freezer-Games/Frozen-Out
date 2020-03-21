using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue;

namespace Scripts.Level.Camera
{
    public class CameraManager : MonoBehaviour
    {

        public GameObject CameraBase;
        public GameObject AuxCamPos;

        [Header("Cameras")]
        public GameObject MainCamera;
        public GameObject CinematicCamera;

        [Header("Camera positions")]
        public Transform NormalPosition;
        
        public Transform CinematicPosition;

        public GameObject BlackBars;

        void Start()
        {
            CameraBase = GameObject.Find("CameraBase");
            AuxCamPos = GameObject.Find("AuxCamPos");
            NormalPosition = AuxCamPos.transform.GetChild(1);
            CinematicPosition = AuxCamPos.transform.GetChild(0);
        }

        public void ToNormal()
        {
            BlackBars.GetComponent<CinematicBars>().Hide(.3f);

            ChangeToNormal();

            EnableController();
        }

        public void ToCinematic()
        {
            DisableController();

            ChangeToCinematic();

            BlackBars.GetComponent<CinematicBars>().Show(150, 0.3f);

            CinematicCamera.GetComponent<CineCamAim>()
                .SetTarget((GameObject.FindGameObjectWithTag("Temporal").transform));
        }

        private void ChangeToNormal()
        {
            MainCamera.SetActive(true);
            CinematicCamera.SetActive(false);
            UnityEngine.Camera.main.transform.position = NormalPosition.position;
        }

        private void ChangeToCinematic()
        {
            MainCamera.SetActive(false);
            CinematicCamera.SetActive(true);
        }

        public void DisableController()
        {
            CameraBase.GetComponent<CameraFollow>().enabled = false;
            AuxCamPos.GetComponent<RotateAround>().enabled = false;
        }

        public void EnableController()
        {
            CameraBase.GetComponent<CameraFollow>().enabled = true;
            AuxCamPos.GetComponent<RotateAround>().enabled = true;
        }

    }
}