using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Button ContinueButton;
    public Button SaveButton;
    public Button LoadButton;
    public Button RestartButton;
    public Button ExitButton;
    public Canvas MenuCanvas;
    public Canvas LoadCanvas;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        LocalizationManagerMenuPausa.instance.LoadLocalizedText("Menu_pausa_Default.json");
        MenuCanvas.enabled = false;
        LoadCanvas.enabled = false;
        ContinueButton.onClick.AddListener(CloseOpenMenu);
        SaveButton.onClick.AddListener(SaveGame);
        LoadButton.onClick.AddListener(LoadGame);
        RestartButton.onClick.AddListener(Restart);
        ExitButton.onClick.AddListener(exit);

    }

    void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void SaveGame() { }

    void LoadGame()
    {

        LoadCanvas.enabled = true;

    }

    void exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }


    void CloseOpenMenu()
    {
        
        MenuCanvas.enabled = !MenuCanvas.enabled;
        if (MenuCanvas.enabled == true) { Time.timeScale = 0; Cursor.visible = true; Cursor.lockState = CursorLockMode.None; } else { Time.timeScale = 1; Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseOpenMenu();
        }
    }
}
