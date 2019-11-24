using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 10.0f;
    private const float Y_ANGLE_MAX = 75.0f;
    private const float INIT_CAM_OFFSET = 10.0f;
    public Transform lookAt;
    public Transform camTransform;
    public float distance = 10.0f;
    [HideInInspector] public GameObject endOfRay;

    private Camera cam;
    
    private float currentX;
    private float currentY;
    private float sensivityX = 4.0f;
    private float sensivityY = 1.0f;

    void Start() {
        endOfRay = GameObject.Find("End of ray");

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
        Vector3 dir = new Vector3(0, 0, -distance);
        Vector3 sataticDir = new Vector3 (0, 0, -INIT_CAM_OFFSET);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        camTransform.position = lookAt.position + rotation * dir;
        endOfRay.transform.position = lookAt.position + rotation * sataticDir;

        camTransform.LookAt(lookAt.position);
    }

    public void resetDistance() {
        distance = INIT_CAM_OFFSET;
    }
}
