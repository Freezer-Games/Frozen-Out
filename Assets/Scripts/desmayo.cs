using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class desmayo : MonoBehaviour
{
    public Canvas LoadingCanvas;

    private void OnTriggerEnter(Collider other)
    {
        LoadingCanvas.enabled = true;
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

}
