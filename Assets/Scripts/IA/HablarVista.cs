﻿using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;
using System.Collections;
using System.Collections.Generic;

public class HablarVista : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private VariableStorageYarn storageYarn;
    private NavMeshAgent agent;
    [System.NonSerialized]
    public bool movido = false;
    [System.NonSerialized]
    public bool hablado = true;
    private Transform player;
    [SerializeField]
    private Transform puesto;
    public string comprobador;
    //private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        storageYarn = FindObjectOfType<VariableStorageYarn>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        agent = GetComponent<NavMeshAgent>();
        //animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(storageYarn.GetValue(comprobador));
        if (storageYarn.GetValue(comprobador) == Yarn.Value.NULL)
        {
            List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
            if (visibles.Count > 0 && movido == false)
            {
                player = GameObject.Find("POL").transform;
                agent.destination = player.position;
                hablado = false;
                movido = true;
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance && hablado == false)
        {
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
            hablado = true;
        }
        if (movido == true && hablado == true && !dialogueSystemYarn.isDialogueRunning) { agent.stoppingDistance = 0; agent.destination = puesto.position; }
    }
}