using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuttiMomento : MonoBehaviour
{
    public GameObject tutti;

     GameObject player;
    public Transform initPos, finalPos;

    private bool repeated = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (!repeated && other.gameObject.tag == "Player") {
            repeated = true;

            player = other.gameObject;
            player.GetComponent<PlayerController>().enabled = false;

            StartCoroutine(WatchingTutti(player));
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    IEnumerator WatchingTutti(GameObject other)
    {
        Instantiate(tutti, initPos.position, initPos.rotation);

        float step = (finalPos.position - initPos.position).magnitude;

        tutti.transform.position = Vector3.MoveTowards(tutti.transform.position, finalPos.position, step);
        
        yield return new WaitForSeconds(5.0f);
    }
}
