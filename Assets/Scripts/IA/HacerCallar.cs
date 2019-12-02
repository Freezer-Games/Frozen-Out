using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class HacerCallar : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    bool hablar = false;
    string estado = "hola";
    NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        if (hablar)
        {
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
            hablar = false;
        }
        else if (visibles.Count > 0 && cercanos.Count < 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia")
        {
            agent.destination = visibles[0].position;
        }
        else if (cercanos.Count == 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia" && dialogueSystemYarn.currentNodeName != null)
        {
            dialogueSystemYarn.Stop();
            hablar = true;
        }
        if (cercanos.Count == 2)
        {
            agent.destination = gameObject.transform.position;
        }
    }
}
