using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Assets.Scripts.Dialogue;


public class margenes : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private GameObject player;
    public margenes instance;
    public bool mensaje = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        player = GameObject.Find("POL");
    }
    private void Update()
    {
        if (mensaje)
        {
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
            mensaje = false;
        }
    }
}
