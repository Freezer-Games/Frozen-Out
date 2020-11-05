using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CamTrigger : TriggerBase
    {
        public CameraController CameraController;
        public CinemachineVirtualCamera NextCamera;
        public CinemachineVirtualCamera CurrentCamera;

        [SerializeField] bool Unidirectional;

        private void Start()
        {
            CameraController.CameraChange += (send, args) => SwitchCamVarName(args);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(PlayerTag))
            {
                CameraController.ChangePriorities(NextCamera);
            }
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
