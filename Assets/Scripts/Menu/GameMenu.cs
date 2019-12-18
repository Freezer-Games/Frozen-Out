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
    public Canvas LoadingCanvas;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        LocalizationManagerMenuPausa.instance.LoadLocalizedText("Menu_pausa_Default.json");
        MenuCanvas.enabled = false;
        LoadCanvas.enabled = false;
        LoadingCanvas.enabled = false;
        ContinueButton.onClick.AddListener(CloseOpenMenu);
        SaveButton.onClick.AddListener(SaveGame);
        LoadButton.onClick.AddListener(LoadGame);
        RestartButton.onClick.AddListener(Restart);
        ExitButton.onClick.AddListener(exit);

    }

    void Restart()
    {
        GameManager.instance.menuopen = false;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SaveGame() {

        GameObject.Find("GameInfo").GetComponent<Game>().SaveGame();

    }

    void LoadGame()
    {
        GameManager.instance.menuopen = false;
        GameObject.Find("GameInfo").GetComponent<Game>().LoadGame();//load last savegame
        LoadingCanvas.enabled = true;
    }

    IEnumerator LoadLevel(int level)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }

    void exit()
    {
        GameManager.instance.menuopen = false;
        Time.timeScale = 1;
        MenuCanvas.enabled = false;
        LoadingCanvas.enabled = true;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }


    void CloseOpenMenu()
    {
        
        
        if (MenuCanvas.enabled == false && !GameManager.instance.menuopen) { GameManager.instance.menuopen = true; MenuCanvas.enabled = true; Time.timeScale = 0; Cursor.visible = true; Cursor.lockState = CursorLockMode.None; }
        else  if (MenuCanvas.enabled == true && GameManager.instance.menuopen) { Time.timeScale = 1; Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; MenuCanvas.enabled = false; GameManager.instance.menuopen = false; }


    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOpenMenu();
        }
    }
}
