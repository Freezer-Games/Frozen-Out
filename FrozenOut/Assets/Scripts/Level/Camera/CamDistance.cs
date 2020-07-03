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
        public CinemachineVirtualCamera VNearCam;

        public Transform Coll;

        public Transform Player;
        public float farDistance;
        public float nearDistance;
        public bool VNisActive;

        [SerializeField] float Dist;

        void Update()
        {
            Dist = Vector3.Distance(Player.position, GenCam.transform.position);

            AuxCollRotMove(Dist);

            if (VNisActive)
            {
                VNearCam.Priority = 50;
            }
            else
            {
                VNearCam.Priority = 10;

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
                else if (Dist < nearDistance)
                {
                    FarCam.Priority = 20;
                    NearCam.Priority = 30;
                    GenCam.Priority = 20;
                }
            }
        }

        private void AuxCollRotMove(float distance)
        {
            Vector3 genCamPos = GenCam.transform.position;
            Coll.position = new Vector3(genCamPos.x, Coll.position.y, genCamPos.z + 1f);

            var lookPos = Player.position;
            var rotation = Quaternion.LookRotation(lookPos);

            if (distance > 4)
            {
                Coll.rotation = Quaternion.Slerp(Coll.rotation, rotation, 0.1f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("hola");
                VNisActive = true;
            }
        }
    }
}

