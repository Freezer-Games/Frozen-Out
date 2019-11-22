using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform cam;
    public Transform character;
    public Transform leftRay;
    public Transform rightRay;
    public Material transparencia;

    private Dictionary<string,Renderer> viewcolisions = new Dictionary<string, Renderer>();
    private Dictionary<string,Material> materiales = new Dictionary<string, Material>();
    List<string> objetos = new List<string>();

    private Vector3 direction;
    private Vector3 camPosition;
    private Vector3 characterPosition;
    private Vector3 addedCharPos = new Vector3(0, 1.3f, 0);

    // Update is called once per frame
    void Update()
    {
        camPosition = cam.position;
        characterPosition = character.position + addedCharPos;

        direction = characterPosition - camPosition;

        int layerMask = 1 << 9;
        objetos.Clear();

        RaycastHit[] hits;
        hits = Physics.RaycastAll(camPosition, direction, direction.magnitude, layerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            string objeto = hit.transform.gameObject.name;
            Renderer rend = hit.transform.GetComponent<Renderer>();
            if (rend)
            {
                if (!viewcolisions.ContainsKey(objeto))
                {
                    viewcolisions.Add(objeto, rend);
                    materiales.Add(objeto, rend.material);
                    rend.material = transparencia;
                    //Debug.Log("añade");
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
                viewcolisions[key].material = materiales[key];
                viewcolisions.Remove(key);
                materiales.Remove(key);
            } 
            else if (!objetos.Contains(key))
            {
                //Debug.Log("elimina1");
                viewcolisions[key].material = materiales[key];
                viewcolisions.Remove(key);
                materiales.Remove(key);
            }

        }

    }
}
