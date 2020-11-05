using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class ParentCamTrigger : MonoBehaviour
    {
        public CameraController CameraController;
        public CinemachineVirtualCamera NextCamera;
        public CinemachineVirtualCamera CurrentCamera;

        [SerializeField] bool Unidirectional;

        private void Start()
        {
            CameraController.CameraChange += (send, args) => SwitchCamVarName(args);
        }

        public void ChangeCamPriority() 
        {
            CameraController.ChangePriorities(NextCamera);
        }

        public void SwitchCamVarName(CinemachineVirtualCamera camera)
        {
            if (!Unidirectional && camera == NextCamera)
            {
                CinemachineVirtualCamera AuxCamera = NextCamera;
                NextCamera = CurrentCamera;
                CurrentCamera = AuxCamera;
            }
        }
    }
}

