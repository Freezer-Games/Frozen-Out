using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Menu : MonoBehaviour
{

    public Button StartButton;
    public Button ContinueButton;
    public Button LoadButton;
    public Button OptionsButton;
    public Button ExitButton;
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    public Canvas LoadCanvas;
    public Canvas LoadlevelCanvas;
    public Dropdown language;

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists("Assets/StreamingAssets/Menu_Default.json")) { LocalizationManager.instance.LoadLocalizedText("Menu_Default.json"); }
        else if (Application.systemLanguage == SystemLanguage.Spanish) { language.value = 1;}
        else { language.value = 0; }
        StartButton.onClick.AddListener(Lanzar_nivel);
        OptionsButton.onClick.AddListener(Options);
        ExitButton.onClick.AddListener(Exit);
        ContinueButton.onClick.AddListener(continuegame);
        LoadButton.onClick.AddListener(loadgame);

    }

    void loadgame()
    {

        LoadlevelCanvas.enabled = true;

    }

    void continuegame()
    {

        //load last savegame

    }


    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene("sala-de-pruebas", LoadSceneMode.Single);
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