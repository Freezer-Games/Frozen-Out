using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeCameras : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public CinemachineVirtualCamera nextCamera;
    private CinemachineVirtualCamera auxCam;

    public List<GameObject> walls;
    [SerializeField] bool canGoBack;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentCamera.Priority = -10;
            nextCamera.Priority = 12;

            if (walls.Count != 0)
            {
                wallsManagement();
            }

            if (canGoBack)
            {
                auxCam = currentCamera;
                currentCamera = nextCamera;
                nextCamera = auxCam;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void wallsManagement()
    {
        foreach (GameObject o in walls)
        {
            var render = o.GetComponent<Renderer>();

            render.enabled = !render.isVisible;
            //o.SetActive(!o.activeSelf);
        }
    }
}
