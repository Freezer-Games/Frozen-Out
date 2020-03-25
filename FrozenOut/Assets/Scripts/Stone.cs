using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stone : MonoBehaviour
{
    public Canvas canvas;
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            panel.SetActive(false);
        }
    }
}
