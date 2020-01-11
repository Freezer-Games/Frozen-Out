using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public float RotateSpeed = 120f;
    public GameObject ToFollowObj;
    Vector3 FollowPos;
    public float sensitivity = 150f;
    public GameObject PlayerObj;
    public float mouseX;
    public float finalInputX;
    private float rotY = 0.0f;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        finalInputX = mouseX;

        rotY += finalInputX * sensitivity * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
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
