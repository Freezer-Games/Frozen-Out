using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class patrullar : MonoBehaviour
{
    public Transform[] points;
    public int destPoint = 0;
    private NavMeshAgent agent;
    private DialogueRunner dialogueSystemYarn;
    enum Estados { patrullando, perseguir, esperar }
    Estados estado;
    bool hablar = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        agent.autoBraking = false;
        estado = Estados.patrullando;
        animator = gameObject.GetComponent<Animator>();
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
        patrulla(visibles,cercanos);
        
    }
    void patrulla(List<Transform> visibles, List<Transform> cercanos) {
        switch (estado)
        {
            case Estados.patrullando:
                animator.SetBool("isTrooping", true);
                if (!agent.pathPending && agent.remainingDistance < 1f)
                GotoNextPoint();
                if ((visibles.Count > 0 && dialogueSystemYarn.isDialogueRunning && !dialogueSystemYarn.currentNodeName.Contains("Guardia") && !dialogueSystemYarn.currentNodeName.Contains("pensar")) && dialogueSystemYarn.currentNodeName != null)
                { animator.SetBool("isTrooping", false); animator.Play("Sorpresa"); estado = Estados.perseguir; }
                break;

            case Estados.perseguir:
                if (hablar)
                {
                    animator.SetBool("isTrooping", true);
                    dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
                    hablar = false;
                    estado = Estados.esperar;
                }
                else if (visibles.Count > 0 &&
                    cercanos.Count < 2 &&
                    dialogueSystemYarn.isDialogueRunning &&
                    dialogueSystemYarn.currentNodeName.Contains("Guardia"))
                {
                    agent.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0 && dialogueSystemYarn.isDialogueRunning && !dialogueSystemYarn.currentNodeName.Contains("Guardia") && dialogueSystemYarn.currentNodeName != null && !dialogueSystemYarn.currentNodeName.Contains("pensar"))
                {
                    agent.destination = gameObject.transform.position;
                    dialogueSystemYarn.Stop();
                    dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
                    agent.destination = gameObject.transform.position;
                    hablar = true;
                }
                if (visibles.Count == 0 && agent.destination == gameObject.transform.position) { estado = Estados.patrullando; }
                break;

            case Estados.esperar:
                animator.SetBool("isTrooping", false);
                if (cercanos.Count == 0 && visibles.Count == 0) { estado = Estados.patrullando; }
                if ((visibles.Count > 0 && dialogueSystemYarn.isDialogueRunning && !dialogueSystemYarn.currentNodeName.Contains("Guardia") && !dialogueSystemYarn.currentNodeName.Contains("pensar")) && dialogueSystemYarn.currentNodeName != null)
                { animator.Play("Sorpresa"); estado = Estados.perseguir; }
                break;
        }
    }
}