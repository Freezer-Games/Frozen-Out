﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public KeyCode jump { get; set; }
    public KeyCode forward { get; set; }
    public KeyCode backward { get; set; }
    public KeyCode right { get; set; }
    public KeyCode left { get; set; }
    public KeyCode crouch { get; set; }
    public KeyCode interact { get; set; }

    public static GameManager instance;
    public CameraController cameraController; //hacer un cameraManager
    public GameObject playerObject;
    public PlayerManager playerManager;

    void Awake()
    {
        MakeSingleton();
        if (SceneManager.GetActiveScene().buildIndex == 1)
        LocalizationManager.instance.LoadLocalizedText("Trial_level_Default.json");

        AssignKeys();
    }

    void AssignKeys()
    {
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", "W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", "S"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("CrouchKey", "LeftControl"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("InteractKey", "F"));
    }
    void Start()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
#endif
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        if (scene.buildIndex == 0) {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

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
