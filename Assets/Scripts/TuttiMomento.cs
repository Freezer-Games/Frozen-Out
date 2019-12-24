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
        if (!repeated && other.gameObject.tag == "Player") 
        {
            PlayerManager.instance.DisableController();
            PlayerManager.instance.inCinematic = true;

            blackBars.GetComponent<CinematicBars>().Show(150, 0.3f);

            repeated = true;
        
            WatchingTutti();
        }
    }

    public void WatchingTutti()
    {
        Instantiate(tutti, initPos.position, initPos.rotation);

        float step = (finalPos.position - initPos.position).magnitude;

        tutti.transform.position = Vector3.MoveTowards(tutti.transform.position, finalPos.position, step);
        
    }
}
