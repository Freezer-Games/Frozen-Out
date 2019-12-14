using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class desmayo : MonoBehaviour
{
    public GameObject topLid;
    public GameObject bottomLid;
    public Canvas LoadingCanvas;

    private void OnTriggerEnter(Collider other)
    {
        Parpadear();
        StartCoroutine(CargarNivel());
    }
    void Parpadear()
    {
        topLid.GetComponent<Animation>().Play("Parpadeo");
        bottomLid.GetComponent<Animation>().Play("Parpadeo");
    }

    IEnumerator CargarNivel()
    {
        yield return new WaitForSeconds(2);
        LoadingCanvas.enabled = true;
        AsyncOperation async = SceneManager.LoadSceneAsync(3);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }
    }
}




