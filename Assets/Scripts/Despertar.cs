using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despertar : MonoBehaviour
{
    public GameObject topLid;
    public GameObject bottomLid;

    // Start is called before the first frame update
    void Start()
    {
        Parpadear();
    }

    // Update is called once per frame
    void Parpadear()
    {
        topLid.GetComponent<Animation>().Play("Parpadeo");
        bottomLid.GetComponent<Animation>().Play("Parpadeo");
    }
}
