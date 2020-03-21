using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Camera
{
    public class CameraFollow : MonoBehaviour
    {

        public float CameraMoveSpeed = 120.0f;
        public GameObject CameraFollowObj;
        Vector3 FollowPos;
        public float ClampAngle = 80.0f;
        public float InputSensitivity = 150.0f;
        public GameObject CameraObj;
        public GameObject PlayerObj;
        public float CamDistanceXTopPlayer;
        public float CamDistanceYTopPlayer;
        public float CamDistanceZTopPlayer;
        public float MouseX;
        public float MouseY;
        public float FinalInputX;
        public float FinalInputZ;
        public float SmoothX;
        public float SmoothY;
        private float RotationY = 0.0f;
        private float RotationX = 0.0f;
        private float TransitionSpeed = 1.0f;
        private Transform CinemaPos;
        private UnityEngine.Camera Camera;

        void Start()
        {
            Camera = UnityEngine.Camera.main;
            CinemaPos = GameObject.Find("AuxCamPos").transform.GetChild(0);
            Vector3 rot = transform.localRotation.eulerAngles;
            RotationY = rot.y;
            RotationX = rot.x;
        }

        // Update is called once per frame
        void Update()
        {
            
            //para pillar controles de mando hay que crear un axis de esos, cosa easy si sabes la distribucion
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
            FinalInputX = MouseX;
            FinalInputZ = MouseY;

            RotationY += FinalInputX * InputSensitivity * Time.deltaTime;
            RotationX += FinalInputZ * InputSensitivity * Time.deltaTime;

            RotationX = Mathf.Clamp(RotationX, -ClampAngle, ClampAngle);

            Quaternion localRotation = Quaternion.Euler(RotationX, RotationY, 0.0f);
            transform.rotation = localRotation;
        }

        void LateUpdate() 
        {
            CameraUpdater();
        }

        void CameraUpdater() 
        {
            Transform target = CameraFollowObj.transform;

            float step = CameraMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }
}