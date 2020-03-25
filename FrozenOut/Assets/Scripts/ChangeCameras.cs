using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameras : MonoBehaviour
{
    public CinemachineVirtualCamera trackCamera;
    public CinemachineVirtualCamera levelCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trackCamera.Priority = -10;
            levelCamera.Priority = 12;
            gameObject.SetActive(false);
        }
    }
}
