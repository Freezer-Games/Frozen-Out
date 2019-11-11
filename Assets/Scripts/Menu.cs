using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Button StartButton;
    public Button OptionsButton;
    public Button ExitButton;
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void lanzar_nivel()
    {

        SceneManager.LoadScene("sala-de-pruebas");

    }
    void options()
    {

        MainCanvas.enabled = false;
        OptionsCanvas.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        StartButton.onClick.AddListener(lanzar_nivel);
        OptionsButton.onClick.AddListener(options);
    }
}