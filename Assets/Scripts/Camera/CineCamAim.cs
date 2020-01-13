using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCamAim : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        try {
            target = GameObject.FindGameObjectWithTag("Temporal").transform;
            transform.LookAt(target);
        } catch {}
    }

}
