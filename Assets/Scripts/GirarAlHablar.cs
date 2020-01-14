using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class GirarAlHablar : MonoBehaviour
{
    private NavMeshAgent agent;
    private DialogueRunner dialogueSystemYarn;
    private NPCYarn npcYarn;

    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
    }

    void Update()
    {
        if (dialogueSystemYarn.isDialogueRunning && npcYarn.talkToNode == dialogueSystemYarn.currentNodeName) { gameObject.transform.LookAt(GameObject.Find("POL").transform); }
    }
}
