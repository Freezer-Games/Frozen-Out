using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera CurrentVC;
        public Transform Player;
        [SerializeField] float diffPlayerCam;
        [SerializeField] float diffPlyCam4S;
        [SerializeField] float currentDiff;

        public CinemachineVirtualCamera FirstSegmenetVC;
        public CinemachineVirtualCamera FourthSegmentVC;

        void Start()
        {
            CurrentVC = FirstSegmenetVC;
        }

        void Update()
        {
            if (CurrentVC != FirstSegmenetVC)
            {
                currentDiff = CurrentVC.transform.position.y - Player.transform.position.y;

                if (CurrentVC == FourthSegmentVC)
                {
                    if (currentDiff > diffPlyCam4S && currentDiff >= 0)
                    {
                        FirstSegmenetVC.Priority = 30;
                        CurrentVC.Priority = 20;
                        Debug.Log("cambio de camara principal");
                        CurrentVC = FirstSegmenetVC;
                    }
                }
                else if (currentDiff > diffPlayerCam && currentDiff > 0)
                {
                    FirstSegmenetVC.Priority = 30;
                    CurrentVC.Priority = 20;
                    Debug.Log("cambio de camara principal");
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

