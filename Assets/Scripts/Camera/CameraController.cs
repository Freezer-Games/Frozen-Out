using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Objetive")]
    public Transform lookAt;

    [Header("Camera preferences")]
    [SerializeField] private const float Y_ANGLE_MIN = 5.0f;
    [SerializeField] private const float Y_ANGLE_MAX = 80.0f;
    [SerializeField] private float INIT_CAM_OFFSET = 7.0f;

    [HideInInspector] public Transform camTransform;
    [HideInInspector] public float distance;
    [HideInInspector] public GameObject endOfRay;
    private Camera cam;
    private float currentX;
    private float currentY;

    void Start() {
        endOfRay = GameObject.Find("End of ray");
        distance = INIT_CAM_OFFSET;
        camTransform = transform;
        cam = Camera.main;
        endOfRay.transform.position = cam.transform.position;
    }

    void Update() {
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    void LateUpdate() {
        Vector3 dir = new Vector3(0, 2f, -distance);
        Vector3 sataticDir = new Vector3 (0, 2f, -INIT_CAM_OFFSET);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        camTransform.position = lookAt.position + rotation * dir;
        endOfRay.transform.position = lookAt.position + rotation * sataticDir;

        camTransform.LookAt(lookAt.position);
    }

    public void resetDistance() {
        distance = INIT_CAM_OFFSET;
    }
}
