using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuttiMomento : MonoBehaviour
{
    public GameObject tutti;
    private GameObject player;
    public Transform spawnPos, finalPos;
    private bool repeated = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (!repeated && other.gameObject.tag == "Player") 
        {
            PlayerManager.instance.DisableController();
            CameraManager.instance.DisableController();
            PlayerManager.instance.inCinematic = true;

            GameManager.instance.blackBars.GetComponent<CinematicBars>().Show(150, 0.3f);

            repeated = true;
        
            TuttiForScene();
        }
    }

    public void TuttiForScene()
    {
        Instantiate(tutti, spawnPos.position, spawnPos.rotation);
        GameObject.FindGameObjectWithTag("Temporal")
            .GetComponent<TuttiMovement>()
            .SetPoint(finalPos.position);
    }
}
