using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject player;
    public bool inCinematic = false;

    void Awake() 
    {
        MakeSingleton();
        try 
        {
            player = GameObject.Find("Pol");
        } catch {}
    }

    public void DisableController()
    {
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
    }

    public void EnableCopntroller()
    {
        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
    }

    

    public void goToCheckPoint(Transform trans)
    {
        Instantiate(player, trans.position, trans.rotation);
    }


    protected void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
