using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CamTrigger : TriggerBase
    {
        //public CameraController CameraController;
        public CinemachineVirtualCamera NextCamera;
        public CinemachineVirtualCamera CurrentCamera;

        [SerializeField] bool Unidirectional;
        //[SerializeField] bool Disable;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(PlayerTag))
            {
                ChangeCamPriority();  
            }
        }

        void ChangeCamPriority() 
        {
            NextCamera.Priority = 40;
            CurrentCamera.Priority = 20;

            if (!Unidirectional)
            {
                CinemachineVirtualCamera AuxCamera = NextCamera;
                NextCamera = CurrentCamera;
                CurrentCamera = AuxCamera;
            }
        }
    }

}
