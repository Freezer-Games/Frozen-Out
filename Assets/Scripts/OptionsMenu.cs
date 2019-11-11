using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public Button ConfirmButton;
    public Button CancelButton;
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        OptionsCanvas.enabled = false;
    }

    void confirm()
    {

        MainCanvas.enabled = true;
        OptionsCanvas.enabled = false;

    }

    void cancel()
    {

        MainCanvas.enabled = true;
        OptionsCanvas.enabled = false;

    }


    // Update is called once per frame
    void Update()
    {
        ConfirmButton.onClick.AddListener(confirm);
        CancelButton.onClick.AddListener(cancel);
    }
}
