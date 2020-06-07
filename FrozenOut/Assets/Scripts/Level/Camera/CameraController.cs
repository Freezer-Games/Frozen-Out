using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera CurrentVC;
        private CinemachineVirtualCamera ExchangeVC;
        public Transform Player;
        [SerializeField] float diffPlayerCam;

        public CinemachineVirtualCamera FirstSegmenetVC;

        void Start()
        {
            CurrentVC = FirstSegmenetVC;
        }

        void Update()
        {
            if (CurrentVC != FirstSegmenetVC)
            {
                float currentDiff = CurrentVC.transform.position.y - Player.transform.position.y;

                if (currentDiff >= diffPlayerCam)
                {
                    FirstSegmenetVC.Priority = 30;
                    CurrentVC.Priority = 20;
                    Debug.Log("cambio de camra principal");
                    CurrentVC = FirstSegmenetVC;
                }
            }
        }

        public void SetCurrentVC(CinemachineVirtualCamera vCam)
        {
            CurrentVC = vCam;
        }
    }
}

