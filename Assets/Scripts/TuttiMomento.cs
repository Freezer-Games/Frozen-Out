using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuttiMomento : MonoBehaviour
{
    public GameObject tutti;
    private GameObject player;
    public Transform initPos, finalPos;
    private bool repeated = false;
    public GameObject blackBars;

    private void OnTriggerEnter(Collider other) 
    {
        if (!repeated && other.gameObject.tag == "Player") {
            repeated = true;

            player = other.gameObject;
            player.GetComponent<PlayerController>().enabled = false;

            GameManager.instance.inCinematic = true;

            blackBars.GetComponent<CinematicBars>().Show(150, 0.3f);

            StartCoroutine(WatchingTutti(player));
            //player.GetComponent<PlayerController>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        
    }

    IEnumerator WatchingTutti(GameObject other)
    {
        Instantiate(tutti, initPos.position, initPos.rotation);

        float step = (finalPos.position - initPos.position).magnitude;

        tutti.transform.position = Vector3.MoveTowards(tutti.transform.position, finalPos.position, step);
        
        yield return new WaitForSeconds(5.0f);
    }
}
