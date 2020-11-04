using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class ParentCamTrigger : MonoBehaviour
    {
        public CinemachineVirtualCamera NextCamera;
        public CinemachineVirtualCamera CurrentCamera;

        [SerializeField] bool Unidirectional;

        public void ChangeCamPriority() 
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

