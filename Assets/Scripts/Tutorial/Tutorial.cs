using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class Tutorial : MonoBehaviour
{
    public Text label;
    private GameManager manager;
    enum estadotut {esperar, cam, mov, sal, aga }
    estadotut estado = estadotut.esperar;
    public Canvas canvas;
    private DialogueRunner dialogueSystemYarn;
    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        manager = FindObjectOfType<GameManager>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        label.text = LocalizationManager.instance.GetLocalizedValue("Tut_cam");
    }

    // Update is called once per frame
    void Update()
    {
        if (estado == estadotut.esperar && !dialogueSystemYarn.isDialogueRunning && !dialogueSystemYarn.isDialogueWaiting)
        {
            estado = estadotut.cam;
            canvas.enabled = true;
        } else if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && estado == estadotut.cam)
        {
            label.text = LocalizationManager.instance.GetLocalizedValue("Press") + PlayerPrefs.GetString("ForwardKey", "W") + PlayerPrefs.GetString("LeftKey", "A") + PlayerPrefs.GetString("BackKey", "S") + PlayerPrefs.GetString("RightKey", "D") + LocalizationManager.instance.GetLocalizedValue("Tut_mov");
            estado = estadotut.mov;
        } else if ((Input.GetKeyDown(manager.forward) || Input.GetKeyDown(manager.backward) || Input.GetKeyDown(manager.right) || Input.GetKeyDown(manager.left)) && estado == estadotut.mov)
        {
            label.text = LocalizationManager.instance.GetLocalizedValue("Press") + PlayerPrefs.GetString("jumpKey", "SpaceBar") + LocalizationManager.instance.GetLocalizedValue("Tut_jump");
            estado = estadotut.sal;
        } else if (Input.GetKeyDown(manager.jump) && estado == estadotut.sal)
        {
            label.text = LocalizationManager.instance.GetLocalizedValue("Press") + PlayerPrefs.GetString("CrouchKey", "LeftControl") + LocalizationManager.instance.GetLocalizedValue("Tut_crouch");
            estado = estadotut.aga;
        } else if (Input.GetKeyDown(manager.crouch) && estado == estadotut.aga) { canvas.enabled = false; }
    }
}
