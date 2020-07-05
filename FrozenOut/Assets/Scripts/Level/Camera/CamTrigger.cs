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
        public CinemachineVirtualCamera AreaCamera;

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
            //Activate segment cam
            if (AreaCamera.Priority == 20)
            {
                AreaCamera.Priority = 30;
                SegmentCamera.Priority = 20;

                if (Disable)
                {
                    GetComponent<Collider>().enabled = false;
                }
                
            }
            //Deactivate segment cam
            else 
            {
                AreaCamera.Priority = 20;
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
