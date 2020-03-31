using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioDeMaya : MonoBehaviour
{
    public GameObject initialObject;
    public GameObject swapObject;

    Mesh initialMesh;
    Mesh swapMesh;

    GameObject theTarget;

    // Use this for initialization
    void Start()
    {
        theTarget = initialObject;

        initialMesh = initialObject.GetComponent<MeshFilter>().mesh;
        swapMesh = swapObject.GetComponent<MeshFilter>().sharedMesh;
    }

    public void ChangeToCube()
    {
        theTarget.GetComponent<MeshFilter>().mesh = swapMesh;
    }

    public void ChangeToNormal()
    {
        theTarget.GetComponent<MeshFilter>().mesh = initialMesh;
    }
}
