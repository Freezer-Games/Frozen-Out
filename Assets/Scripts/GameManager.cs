using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance
    {
        get
        {
            return singleton;
        }
    }
    private static readonly GameManager instance = new GameManager();

    private GameManager()
    {
        DontDestroyOnLoad(gameObject);
    }

}
