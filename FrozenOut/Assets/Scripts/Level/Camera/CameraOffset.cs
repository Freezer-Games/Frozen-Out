using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts.Level.Camera
{
    public class CameraOffset : MonoBehaviour
    {
        public CinemachineVirtualCamera Camera;
        public CinemachineCameraOffset Offset;
        public Transform Objective;
        public float distance;

        [Header("Values")]
        public float outside_Y_Offset = 3.6f;
        [SerializeField] private float inner_Y_Offset = -2.5f;
        [SerializeField]  private float distanceToChange = 10f;
        public Vector3 currentOffset;

        private void Update()
        {
            if (Camera.Priority == 40)
            {
                currentOffset = Offset.m_Offset;
                ChangeOffset();
            }
        }

        private void ChangeOffset()
        {
            distance = Vector3.Distance(
                Camera.Follow.position, Objective.position);

            if (distance < distanceToChange)
            {
                Offset.m_Offset =
                    new Vector3(currentOffset.x, Mathf.Lerp(currentOffset.y, inner_Y_Offset, 1f * Time.deltaTime), currentOffset.z);
            }
            else if (distance >= distanceToChange)
            {
                Offset.m_Offset =
                    new Vector3(currentOffset.x, Mathf.Lerp(currentOffset.y, outside_Y_Offset, 1f * Time.deltaTime), currentOffset.z);
            }
        }
    }
}

