using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public Button StartButton;
    public Button OptionsButton;
    public Button ExitButton;
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    public Canvas LoadCanvas;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(Lanzar_nivel);
        OptionsButton.onClick.AddListener(Options);
        ExitButton.onClick.AddListener(Exit);

    }

    IEnumerator LoadLevel()
    {
        
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("sala-de-pruebas");
    }

    void Lanzar_nivel()
    {

        MainCanvas.enabled = false;
        LoadCanvas.enabled = true;
        StartCoroutine(LoadLevel());

    }
    void Options()
    {

        MainCanvas.enabled = false;
        OptionsCanvas.enabled = true;

    }

    private void Exit()
    {
        Application.Quit();
    }


    // Update is called once per frame
    void Update()
    {

    }
}