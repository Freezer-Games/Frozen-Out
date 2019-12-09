using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuttiMomento : MonoBehaviour
{
    public GameObject tutti;

    public GameObject player;
    public Transform initPos, finalPos;
    public float tuttiVel = 5f;

    private bool repeated = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (!repeated && other.gameObject.tag == "Player") {
            repeated = true;

            player = other.gameObject;

            StartCoroutine(WatchingTutti(player));
            

            player.GetComponent<PlayerController>().enabled = true;
            


        }
    }

    IEnumerator WatchingTutti(GameObject other)
    {
        other.GetComponent<PlayerController>().enabled = false;
        Instantiate(tutti, initPos.position, initPos.rotation);

        float step = tuttiVel * Time.deltaTime;

        while (tutti.transform.position != finalPos.position) {  
            tutti.transform.position = Vector3.MoveTowards(tutti.transform.position, finalPos.position, step);
        }

        yield return null;
    }
}
