using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject player;
    public bool inCinematic = false;

    private Animator animator;
    private AudioSource steps;
    private PlayerController playerController;

    void Awake() 
    {
        MakeSingleton();
    }

    void Start()
    {
        player = GameObject.Find("POL");
        steps = player.GetComponent<AudioSource>();
        animator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
    }

    public void DisableController()
    {
        animator.enabled = false;
        steps.Stop();
        playerController.enabled = false;
    }

    public void EnableController()
    {
        player.GetComponent<Animator>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
    }

    

    public void ToCheckPoint(Transform trans)
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
