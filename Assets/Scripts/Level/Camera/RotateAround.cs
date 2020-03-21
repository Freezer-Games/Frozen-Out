using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Camera
{
    public class RotateAround : MonoBehaviour
    {
        public float RotateSpeed = 120f;
        public GameObject ToFollowObj;
        Vector3 FollowPos;
        public float Sensitivity = 150f;
        public GameObject PlayerObj;
        public float MouseX;
        public float FinalInputX;

        private float RotationY = 0.0f;

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