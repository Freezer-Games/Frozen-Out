using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValue;
    public float rotateSpeed;
    public Transform pivot; 
    public float maxViewAngle;
    public float minviewAngle;
    public bool invertY;

    // Start is called before the first frame update
    void Start()
    {
        if (useOffsetValue) 
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //get X pos raton y rota el target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0,horizontal, 0);

        //get X pos raton y rota el pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        //pivot.Rotate(vertical, 0, 0);
        if (invertY) 
        {
            pivot.Rotate(vertical, 0, 0);
        } else 
        {
            pivot.Rotate(-vertical, 0, 0);
        }

        //limit up/down camera rotation
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f) 
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }

        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < minviewAngle) 
        {
            pivot.rotation = Quaternion.Euler(360f + minviewAngle, 0, 0);
        }

        //Mueve camara según rotación actual del target y offset original
        float desiredYAngle = target.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        //transform.position = target.position - offset;

        if (transform.position.y < target.position.y) 
        {
            transform.position = new Vector3(transform.position.x, target.position.y - .5f, target.position.z);
        }

        transform.LookAt(target);
    }
}
