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
        [SerializeField] float diffPlyrCam2;
        [SerializeField] float diffPlyrCam3;
        [SerializeField] float diffPlyrCam4;
        [SerializeField] float currentDiff;

        public CinemachineVirtualCamera FirstSegmenetVC;
        public CinemachineVirtualCamera SecondSegmentVC;
        public CinemachineVirtualCamera ScndSegTLVC;
        public CinemachineVirtualCamera ThirdSegmentVC;
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

                if (CurrentVC == SecondSegmentVC || CurrentVC == ScndSegTLVC)
                {
                    if (currentDiff > diffPlyrCam2 && currentDiff >= 0)
                    {
                        FirstSegmenetVC.Priority = 30;
                        CurrentVC.Priority = 20;
                        CurrentVC = FirstSegmenetVC;
                    }
                }
                else if (CurrentVC == ThirdSegmentVC)
                {
                    if (currentDiff > diffPlyrCam3 && currentDiff >= 0)
                    {
                        FirstSegmenetVC.Priority = 30;
                        CurrentVC.Priority = 20;
                        CurrentVC = FirstSegmenetVC;
                    }
                }
                else if (CurrentVC == FourthSegmentVC)
                {
                    if (currentDiff > diffPlyrCam4 && currentDiff >= 0)
                    {
                        FirstSegmenetVC.Priority = 30;
                        CurrentVC.Priority = 20;
                        CurrentVC = FirstSegmenetVC;
                    }
                }
            }
        }

        public void SetCurrentVC(CinemachineVirtualCamera vCam)
        {
            CurrentVC = vCam;
        }
    }
}

