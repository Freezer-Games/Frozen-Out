using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform character;
    public Transform leftRay;
    public Transform rightRay;
    public Material transparencia;

    private Dictionary<string,Transform> viewcolisions = new Dictionary<string, Transform>();
    private Dictionary<string,Material> materiales = new Dictionary<string, Material>();
    List<string> objetos = new List<string>();

    private Camera cam;
    private Vector3 direction;
    private Vector3 camPosition;
    private Transform initPos;
    private Vector3 characterPosition;
    private float newCamDistance;
    private Vector3 addedCharacterPos = new Vector3(0, 1.3f, 0);

    void Start() 
    {
        cam = Camera.main;
        initPos = cam.GetComponent<BetterCamera>().endOfRay.transform;
    }
    void Update()
    {
        initPos = cam.GetComponent<BetterCamera>().endOfRay.transform;
        characterPosition = character.position + addedCharacterPos;

        direction = initPos.position - characterPosition;

        int layerMask = 1 << 9;
        objetos.Clear();

        RaycastHit[] hits;
        hits = Physics.RaycastAll(characterPosition, direction, direction.magnitude, layerMask);

        try {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                string objeto = hit.transform.gameObject.name;
                Transform pos = hit.transform;
                if (hit.transform.gameObject.tag == "Wall") 
                {
                    newCamDistance = (hits[0].transform.position  - characterPosition).magnitude;

                    cam.GetComponent<BetterCamera>().distance = newCamDistance + 1.0f;

                    viewcolisions.Add(objeto, pos);
                    materiales.Add(objeto, pos.GetComponent<Renderer>().material);
                    pos.GetComponent<MeshRenderer>().enabled = false;
                }
                else 
                {
                    if (pos)
                    {
                        if (!viewcolisions.ContainsKey(objeto))
                        {
                            viewcolisions.Add(objeto, pos);
                            materiales.Add(objeto, pos.GetComponent<Renderer>().material);
                            pos.GetComponent<Renderer>().material = transparencia;
                            //Debug.Log("añade");
                        }
                    }
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
                    if (viewcolisions[key].gameObject.tag == "Wall") {
                        cam.GetComponent<BetterCamera>().distance = direction.magnitude;
                        viewcolisions[key].GetComponent<MeshRenderer>().enabled = true;
                        viewcolisions.Remove(key);
                        materiales.Remove(key);
                    }
                    else 
                    {
                        viewcolisions[key].GetComponent<Renderer>().material = materiales[key];
                        viewcolisions.Remove(key);
                        materiales.Remove(key);
                    }        
                } 
                else if (!objetos.Contains(key))
                {
                    if (viewcolisions[key].gameObject.tag == "Wall") {
                        cam.GetComponent<BetterCamera>().resetDistance();
                        viewcolisions[key].GetComponent<MeshRenderer>().enabled = true;
                        viewcolisions.Remove(key);
                        materiales.Remove(key);
                    }
                    else 
                    {
                        viewcolisions[key].GetComponent<Renderer>().material = materiales[key];
                        viewcolisions.Remove(key);
                        materiales.Remove(key);
                    }
                    //Debug.Log("elimina1");  
                }
            }
        } catch {}
    }
}

