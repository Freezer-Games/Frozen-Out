using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLoad : MonoBehaviour
{
    public Canvas LoadCanvas;
    public Canvas LoadingScreen;
    public Button LoadButton;
    public Button CancelButton;
    public Scrollbar hscroll;

    // Start is called before the first frame update
    void Start()
    {

        LoadCanvas.enabled = false;
        CancelButton.onClick.AddListener(Cancel);
        hscroll.enabled = false;
        LoadButton.enabled = false;

    }

    void Cancel()
    {

        LoadCanvas.enabled = false;

    }
}
