using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CamTrigger : TriggerBase
    {
        public CameraController CameraController;
        public CinemachineVirtualCamera SegmentCamera;
        public CinemachineVirtualCamera PreviousCamera;

        [SerializeField] bool IsMain;
        [SerializeField] bool Disable;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(PlayerTag))
            {
                ChangeCamPriority();  
            }
        }

        void ChangeCamPriority() 
        {
            /*
                Activate the previous camera of the new zone, 
                it used to be the camera of the lower zone
            */
            if (PreviousCamera.Priority == 20)
            {
                PreviousCamera.Priority = 30;
                SegmentCamera.Priority = 20;

                if (Disable)
                {
                    GetComponent<Collider>().enabled = false;
                }
            }
            //Activate the camera of the segment the player is going
            else 
            {
                PreviousCamera.Priority = 20;
                SegmentCamera.Priority = 40;

                //That's mean the segment camera is the main of each segment
                if (IsMain)
                {
                    CameraController.SetCurrentVC(SegmentCamera);
                }
            }
        }
    }

}
