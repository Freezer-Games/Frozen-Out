using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class GirarAlHablar : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private NPCYarn npcYarn;

    private void Start()
    {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        npcYarn = GetComponent<NPCYarn>();
    }

    void Update()
    {
        if (dialogueSystemYarn.isDialogueRunning && npcYarn.talkToNode == dialogueSystemYarn.currentNodeName) { gameObject.transform.LookAt(GameObject.Find("POL").transform); }
    }
}
