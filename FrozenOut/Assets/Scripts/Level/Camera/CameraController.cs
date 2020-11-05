using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace Scripts.Level.Camera
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineVirtualCamera[] LevelCameras;

        public void ChangePriorities(CinemachineVirtualCamera current)
        {
            foreach (CinemachineVirtualCamera camera in LevelCameras)
            {
                if (camera == current)
                {
                    camera.Priority = 40;
                }
                else
                {
                    camera.Priority = 20;
                }
            }
            OnCameraChange(current);
        }

        public event EventHandler<CinemachineVirtualCamera> CameraChange;

        public void OnCameraChange(CinemachineVirtualCamera camera)
        {
            CameraChange?.Invoke(this, camera);
        }
    }
}

