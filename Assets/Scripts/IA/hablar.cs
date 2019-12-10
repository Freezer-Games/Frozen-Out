using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class hablar : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private NavMeshAgent agent;
    public Transform player;
    private bool hablado = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && hablado == false)
        {
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
            hablado = true;
        }
    }
}
