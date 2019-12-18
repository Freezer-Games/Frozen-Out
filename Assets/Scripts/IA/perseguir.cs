using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class perseguir : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;

    // Update is called once per frame
    void Update()
    {
        dialogueSystemYarn = GetComponent<DialogueRunner>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        if (visibles.Count>0 && cercanos.Count<2) { agent.destination = visibles[0].position; }
        else if (cercanos.Count == 2) {
            agent.destination = gameObject.transform.position;
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
        }
    }
}
