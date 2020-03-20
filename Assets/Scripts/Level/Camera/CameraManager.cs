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

        void Start()
        {
            CameraBase = GameObject.Find("CameraBase");
            AuxCamPos = GameObject.Find("AuxCamPos");
            NormalPosition = AuxCamPos.transform.GetChild(1);
            CinematicPosition = AuxCamPos.transform.GetChild(0);
        }

        public void ChangeToNormal()
        {
            Camera.main.transform.position = NormalPosition.position;
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

        public void ToNormalCamera()
        {
            MainCamera.SetActive(true);
            CinematicCamera.SetActive(false);
        }

        public void ToCinemaCamera()
        {
            MainCamera.SetActive(false);
            CinematicCamera.SetActive(true);
        }

    }
}