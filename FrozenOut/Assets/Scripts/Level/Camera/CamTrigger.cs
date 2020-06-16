using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CamTrigger : MonoBehaviour
    {
        public CameraController CameraController;
        public CinemachineVirtualCamera SegmentCamera;
        public CinemachineVirtualCamera AreaCamera;

        [SerializeField] bool IsMain;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ChangeCamPriority();  
            }
        }

        void ChangeCamPriority() 
        {
            //Is active
            if (AreaCamera.Priority == 20)
            {
                SegmentCamera.Priority = 20;
                AreaCamera.Priority = 30;
            }
            //Isn't active
            else 
            {
                SegmentCamera.Priority = 40;
                AreaCamera.Priority = 20;

                //That's mean the segment camera is the main of each segment
                if (IsMain)
                {
                    CameraController.SetCurrentVC(SegmentCamera);
                }
            }
        }
    }

}
