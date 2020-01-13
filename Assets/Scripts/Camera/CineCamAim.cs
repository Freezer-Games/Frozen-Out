using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCamAim : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Transform pos;

    // Update is called once per frame
    void Update()
    {
        transform.position = pos.position;
        try {
            transform.LookAt(target);
        } catch {}
    }

    public void SetTarget(Transform t) 
    {
        target = t;
    }

}
