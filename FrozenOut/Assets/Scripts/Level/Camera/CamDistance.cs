using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CamDistance : MonoBehaviour
    {
        public CinemachineVirtualCamera GenCam;
        public CinemachineVirtualCamera FarCam;
        public CinemachineVirtualCamera NearCam;

        public Transform Player;
        public float farDistance;
        public float nearDistance;

        void Update()
        {
            float Dist = Vector3.Distance(Player.position, GenCam.transform.position);

            if (Input.GetKeyDown(KeyCode.L)) Debug.Log(Dist);

            if (Dist > farDistance)
            {
                FarCam.Priority = 30;
                GenCam.Priority = 20;
                NearCam.Priority = 20;
            }
            else if (Dist < farDistance && Dist > nearDistance)
            {
                FarCam.Priority = 20;
                NearCam.Priority = 20;
                GenCam.Priority = 30;
            }
            else if (Dist  < nearDistance)
            {
                FarCam.Priority = 20;
                NearCam.Priority = 30;
                GenCam.Priority = 20;
            }
        }
    }
}

