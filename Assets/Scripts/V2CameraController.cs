using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float smoothSpeed = 0.5f;

    Quaternion targetRotation;
    Vector3 targetPos;
    bool smoothRotating = false;

    void Update() {
        MoveWithTarget();
        LookAtTarget();

        
    }

    void MoveWithTarget() {
        targetPos = target.position + offsetPos;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void LookAtTarget() {
        target.rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation )
    }



    /*mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -17, 60); */

}
