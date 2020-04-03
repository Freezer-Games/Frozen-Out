using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        public CameraManager CameraManager;
        
        public float CameraMoveSpeed = 120.0f;
        public float ClampAngle = 80.0f;
        public float InputSensitivity = 150.0f;
        public float MouseX;
        public float MouseY;
        public float FinalInputX;
        public float FinalInputZ;

        private float RotationY = 0.0f;
        private float RotationX = 0.0f;

        private GameObject PlayerCameraPointObject => CameraManager.GetPlayerCameraPointObject();
        private UnityEngine.Camera Camera => CameraManager.GetMainCamera();

        void Start()
        {
            Vector3 rotation = transform.localRotation.eulerAngles;
            RotationY = rotation.y;
            RotationX = rotation.x;
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
            Transform target = PlayerCameraPointObject.transform;

            float step = CameraMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        
    }
}