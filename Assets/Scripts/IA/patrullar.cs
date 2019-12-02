using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class patrullar : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    string estado = "patrullando";
    private DialogueRunner dialogueSystemYarn;
    bool hablar = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        agent.autoBraking = false;
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        switch (estado)
        {
            case "patrullando":
                if (!agent.pathPending && agent.remainingDistance < 1f)
                    GotoNextPoint();
                if (visibles.Count > 0 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia")
                { estado = "perseguir"; }
                break;

            case "perseguir":
                if (hablar)
                {
                    dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
                    hablar = false;
                    estado = "esperar";
                }
                else if (visibles.Count > 0 && cercanos.Count < 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia")
                {
                    agent.destination = visibles[0].position;
                }
                else if (cercanos.Count == 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia" && dialogueSystemYarn.currentNodeName != null)
                {
                    print(dialogueSystemYarn.currentNodeName);
                    dialogueSystemYarn.Stop();
                    hablar = true;
                }
                if (cercanos.Count == 2)
                {
                    agent.destination = gameObject.transform.position;
                }
                break;

            case "esperar":
                if (cercanos.Count == 0 && visibles.Count == 0) { estado = "patrullando"; }
                break;
        }
    }
}