﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class HacerCallar : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private DialogueUIYarn dialogUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dialogUI = FindObjectOfType<DialogueUIYarn>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        if (visibles.Count > 0 && cercanos.Count < 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia") { agent.destination = visibles[0].position; }
        else if (cercanos.Count == 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia") {
            print(dialogueSystemYarn.currentNodeName);
            dialogueSystemYarn.Stop();
            dialogUI.dialogueBoxGUI.SetActive(false);
            //dialogueSystemYarn.RunDialogue("Guardia");
            //StartCoroutine(dialogueSystemYarn.RunDialogue("Guardia"));
            //dialogueSystemYarn.StartDialogue("Guardia");
        }
        if (cercanos.Count == 2) { agent.destination = gameObject.transform.position; }
    }
}