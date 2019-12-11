using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColl : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    void Awake() 
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 desiredCamPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCamPos, out hit)) 
        {   
            if (hit.collider.gameObject.tag != "Trigger" || hit.collider.gameObject.tag != "NPC")
                distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else 
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
