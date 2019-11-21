using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform cam;
    public Transform character;
    public Material transparencia;


    private Vector3 direction;
    private Vector3 camPosition;
    private Vector3 characterPosition;

    private Renderer onCol;

    private Material oldMaterial;


    private bool check;

    void Start() {
        check=false;
    }

    // Update is called once per frame
    void Update()
    {
        camPosition = cam.position;
        characterPosition = character.position;

        direction = characterPosition - camPosition;

        int layerMask = 1 << 9;

        RaycastHit hit;

        if (Physics.Raycast(camPosition, direction, out hit, direction.magnitude, layerMask))
        {
            Debug.DrawRay(camPosition, direction * hit.distance, Color.yellow);


            if (!check) {
                 oldMaterial = hit.transform.gameObject.GetComponent<Renderer>().material;
                 onCol = hit.transform.gameObject.GetComponent<Renderer>();
                 onCol.material = transparencia;
                 Debug.Log(oldMaterial);
                 check = true;
            }


            hit.transform.gameObject.GetComponent<Renderer>().material = transparencia;
            //hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
         

        }
        else
        {
            Debug.DrawRay(camPosition, direction * 1000, Color.blue);

            if (check) {
                onCol.material = oldMaterial;
                check = false;
            }
            Debug.Log("Did not Hit");
        }
    }
}
