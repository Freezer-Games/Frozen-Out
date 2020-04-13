using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Camera
{
    public class RotateAround : MonoBehaviour
    {
        public CameraManager CameraManager;

        public float RotateSpeed = 120f;
        public float Sensitivity = 150f;

        private float MouseX;
        private float FinalInputX;
        private float RotationY = 0.0f;

        private GameObject ToFollowObj => CameraManager.GetPlayerObject();

        void Start()
        {
            Vector3 rot = transform.localRotation.eulerAngles;
            RotationY = rot.y;
        }

        void Update()
        {
            MouseX = Input.GetAxis("Mouse X");
            FinalInputX = MouseX;

            RotationY += FinalInputX * Sensitivity * Time.deltaTime;

            Quaternion localRotation = Quaternion.Euler(0.0f, RotationY, 0.0f);
            transform.rotation = localRotation;
        }

        void LateUpdate() 
        {
            RotateUpdater(); 
        }

        void RotateUpdater()
        {
            Transform target = ToFollowObj.transform;

            float step = RotateSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }

    }
}