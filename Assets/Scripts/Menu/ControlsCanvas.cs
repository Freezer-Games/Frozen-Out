using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsCanvas : MonoBehaviour
{

    [SerializeField]
    Transform menuPanel;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    // Start is called before the first frame update
    void Start()
    {
        waitingForKey=false;

        for (int i = 0; i<menuPanel.childCount; i++)
        {
            if (menuPanel.GetChild(i).name=="ForwardKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.forward.ToString();
            }else if (menuPanel.GetChild(i).name=="BackKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.backward.ToString();
            }else if (menuPanel.GetChild(i).name=="LeftKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.left.ToString();
            }else if (menuPanel.GetChild(i).name=="RightKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.right.ToString();
            }else if (menuPanel.GetChild(i).name=="JumpKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.jump.ToString();
            }else if (menuPanel.GetChild(i).name == "CrouchKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.crouch.ToString();
            }else if (menuPanel.GetChild(i).name == "InteractKey"){
                menuPanel.GetChild(i).GetComponentInChildren<Text>().text = GameManager.instance.interact.ToString();
            }

        }

    }

    void OnGUI()
    {
        keyEvent = Event.current;

        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if(!waitingForKey){StartCoroutine(AssignKey(keyName));}
    }

    public void sendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while(!keyEvent.isKey){yield return null;}

    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey=true;

        yield return WaitForKey();
        switch(keyName)
        {
            case "forward":
                GameManager.instance.forward = newKey;
                buttonText.text = GameManager.instance.forward.ToString();
                PlayerPrefs.SetString("forwardKey", GameManager.instance.forward.ToString());
                break;
            case "backward":
                GameManager.instance.backward = newKey;
                buttonText.text = GameManager.instance.backward.ToString();
                PlayerPrefs.SetString("backwardKey", GameManager.instance.backward.ToString());
                break;
            case "right":
                GameManager.instance.right = newKey;
                buttonText.text = GameManager.instance.right.ToString();
                PlayerPrefs.SetString("rightKey", GameManager.instance.right.ToString());
                break;
            case "left":
                GameManager.instance.left = newKey;
                buttonText.text = GameManager.instance.left.ToString();
                PlayerPrefs.SetString("leftKey", GameManager.instance.left.ToString());
                break;
            case "jump":
                GameManager.instance.jump = newKey;
                buttonText.text = GameManager.instance.jump.ToString();
                PlayerPrefs.SetString("jumpKey", GameManager.instance.jump.ToString());
                break;
            case "crouch":
                GameManager.instance.crouch = newKey;
                buttonText.text = GameManager.instance.crouch.ToString();
                PlayerPrefs.SetString("CrouchKey", GameManager.instance.crouch.ToString());
                break;
            case "interact":
                GameManager.instance.interact = newKey;
                buttonText.text = GameManager.instance.interact.ToString();
                PlayerPrefs.SetString("InteractKey", GameManager.instance.interact.ToString());
                break;

        }

        yield return null;
    }

}
