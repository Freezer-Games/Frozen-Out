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
        ControlsCanvas.enabled = false;
        ConfirmButton.onClick.AddListener(confirm);
        CancelButton.onClick.AddListener(cancel);
        GameButton.onClick.AddListener(Game);
        AudioButton.onClick.AddListener(Audio);
        GraphicsButton.onClick.AddListener(Graphics);
        ControlsButton.onClick.AddListener(Controls);
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

    void Game()
    {

        GameCanvas.enabled = true;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.enabled = false;

    }

    void Audio()
    {

        GameCanvas.enabled = false;
        AudioCanvas.enabled = true;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.enabled = false;

    }

    void Graphics()
    {

        GameCanvas.enabled = false;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = true;
        ControlsCanvas.enabled = false;

    }

    void Controls()
    {

        GameCanvas.enabled = false;
        AudioCanvas.enabled = false;
        GraphicsCanvas.enabled = false;
        ControlsCanvas.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
