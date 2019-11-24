using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class OptionsMenu : MonoBehaviour
{

    public Button ConfirmButton;
    public Button CancelButton;
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    public Button GameButton;
    public Canvas GameCanvas;
    public Button AudioButton;
    public Canvas AudioCanvas;
    public Button GraphicsButton;
    public Canvas GraphicsCanvas;
    public Button ControlsButton;
    public Canvas ControlsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        OptionsCanvas.enabled = false;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.transform.gameObject.SetActive(false);
        ConfirmButton.onClick.AddListener(Confirm);
        CancelButton.onClick.AddListener(Cancel);
        GameButton.onClick.AddListener(Game);
        AudioButton.onClick.AddListener(Audio);
        GraphicsButton.onClick.AddListener(Graphics);
        ControlsButton.onClick.AddListener(Controls);
    }

    void Confirm()
    {

        MainCanvas.enabled = true;
        OptionsCanvas.enabled = false;
        SaveChanges();

    }

    void Cancel()
    {

        MainCanvas.enabled = true;
        OptionsCanvas.enabled = false;

    }

    void SaveChanges()
    {
        if (PlayerPrefs.GetString("Language") == "Es") { EditorUtility.DisplayDialog("Confirmar", "¿estas seguro de que quieres guardar los cambios?", "confirmar", "cancelar"); }
        else { EditorUtility.DisplayDialog("Confirm", "Are you sure you want to save the changes?", "confirm", "cancel"); }
    }

    void Game()
    {

        GameCanvas.enabled = true;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.transform.gameObject.SetActive(false);

    }

    void Audio()
    {

        GameCanvas.enabled = false;
        AudioCanvas.enabled = true;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.transform.gameObject.SetActive(false);

    }

    void Graphics()
    {

        GameCanvas.enabled = false;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = true;
        ControlsCanvas.transform.gameObject.SetActive(false);

    }

    void Controls()
    {

        GameCanvas.enabled = false;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.transform.gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
