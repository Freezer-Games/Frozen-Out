using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorFacingScreen : MonoBehaviour
{
	Transform cameraTransform;
	
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
	
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
