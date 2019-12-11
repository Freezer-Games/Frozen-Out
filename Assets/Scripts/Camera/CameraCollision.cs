using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    /*[Header("Ray start")]
    public Transform character;
    public Transform auxCamPos;

    public Material transparencia;

    private Dictionary<string,Transform> viewcolisions = new Dictionary<string, Transform>();
    private Dictionary<string,Material> materiales = new Dictionary<string, Material>();
    List<string> objetos = new List<string>();

    private Camera cam;
    private Vector3 direction;
    private Vector3 camPosition;
    private Transform initPos;
    private Vector3 characterPosition;
    private RaycastHit[] hits;
    private float newCamDistance;
    private Vector3 addedCharacterPos = new Vector3(0, 1.3f, 0);
    private int layerMask = 1 << 9;

    void Start() 
    {
        cam = Camera.main;
        initPos = cam.GetComponent<CameraController>().endOfRay.transform;
    }
    void Update()
    {
        initPos = cam.GetComponent<CameraController>().endOfRay.transform;
        characterPosition = character.position + addedCharacterPos;

        direction = initPos.position - characterPosition;
        objetos.Clear();

        RayTracing(); 
    }

    private  void RayTracing() {
        hits = Physics.RaycastAll(characterPosition, direction, direction.magnitude, layerMask);

        try {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                string objeto = hit.transform.gameObject.name;
                Transform pos = hit.transform;
                if (hit.transform.gameObject.tag == "Wall")
                {
                    ZoomInCamera(pos, hit, objeto);
                }
                else 
                {
                    MakeItTransparent(pos, hit, objeto);
                }  
                //Debug.Log("hit");
                objetos.Add(objeto);
            }

            List<string> keys = new List<string>(viewcolisions.Keys);

            foreach (string key in keys)
            {
                if (hits.Length == 0)
                {
                    //Debug.Log("elimina0");
                    if (viewcolisions[key].gameObject.tag == "Wall") 
                    {
                        ReleaseZoom(key);
                    }
                    else 
                    {
                        ReleaseMaterial(key);
                    }        
                } 
                else if (!objetos.Contains(key))
                {
                    if (viewcolisions[key].gameObject.tag == "Wall") 
                    {
                        ReleaseZoom(key);
                    }
                    else 
                    {
                        ReleaseMaterial(key);
                    }
                    //Debug.Log("elimina1");  
                }
            }
        } catch {}
    }

    private void ZoomInCamera(Transform pos, RaycastHit hit, string objeto) 
    {
        newCamDistance = (hits[0].point  - characterPosition).magnitude;

        if (newCamDistance <= 3f) 
        {
            cam.transform.position = auxCamPos.position;
        }
        else 
        {
            cam.GetComponent<CameraController>().distance = newCamDistance + 3.0f;
        }
        viewcolisions.Add(objeto, pos);
        materiales.Add(objeto, pos.GetComponent<Renderer>().material);
        pos.GetComponent<MeshRenderer>().enabled = false;
    }

    private void MakeItTransparent(Transform pos, RaycastHit hit, string objeto) 
    {
        if (pos)
        {
            if (!viewcolisions.ContainsKey(objeto))
            {
                viewcolisions.Add(objeto, pos);
                materiales.Add(objeto, pos.GetComponent<Renderer>().material);
                pos.GetComponent<Renderer>().material = transparencia;
            }
        }
    }

    private void ReleaseZoom(string key) 
    {
        cam.GetComponent<CameraController>().distance = direction.magnitude;
        viewcolisions[key].GetComponent<MeshRenderer>().enabled = true;
        viewcolisions.Remove(key);
        materiales.Remove(key);
    }

    private void ReleaseMaterial(string key) 
    {
        viewcolisions[key].GetComponent<Renderer>().material = materiales[key];
        viewcolisions.Remove(key);
        materiales.Remove(key);
    }*/
}

