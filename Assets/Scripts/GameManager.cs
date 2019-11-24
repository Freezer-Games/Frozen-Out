using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BetterCamera betterCamera; //hacer un cameraManager
    public GameObject playerObject;
    public PlayerManager playerManager;

    void Awake()
    {
        MakeSingleton();
    }
    void Start()
    {
        
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
