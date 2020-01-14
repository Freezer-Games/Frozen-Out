using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class PatrullaCircular : MonoBehaviour
{
    public Transform center;
    float radius;
    public int points;
    private NavMeshAgent agent;
    Vector3 direction;
    Vector3 destination;
    enum Estados {patrullando, perseguir, esperar}
    Estados estado = Estados.patrullando;
    private DialogueRunner dialogueSystemYarn;
    bool hablar = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        center.LookAt(gameObject.transform.position);
        radius = Vector3.Distance(gameObject.transform.position, center.transform.position);
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        animator = gameObject.GetComponent<Animator>();
    }

    void GotoNextPoint()
    {
        if (points == 0) { return; }
        int angle = 360 / points;
        center.Rotate(new Vector3(0, angle, 0));
        direction = center.forward;
        destination = new Vector3(center.position.x + radius * direction.x, center.position.y, center.position.z + radius * direction.z);
        agent.destination = destination;
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        switch (estado) {

            case Estados.patrullando:
                if (!agent.pathPending && agent.remainingDistance < 1f)
                    GotoNextPoint();
                if ((visibles.Count > 0 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName.Contains("Guardia") && !dialogueSystemYarn.currentNodeName.Contains("pensar")) && dialogueSystemYarn.currentNodeName != null)
                { animator.Play("Sorpresa"); estado = Estados.perseguir; }
                break;

            case Estados.perseguir:
                if (hablar)
                {
                    dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
                    hablar = false;
                    estado = Estados.esperar;
                }
                else if (visibles.Count > 0 && cercanos.Count < 2 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName.Contains("Guardia"))
                {
                    agent.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0 && dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName.Contains("Guardia") && dialogueSystemYarn.currentNodeName != null && !dialogueSystemYarn.currentNodeName.Contains("pensar"))
                {
                    agent.destination = gameObject.transform.position;
                    dialogueSystemYarn.Stop();
                    animator.Play("Enfado_Entrada");
                    dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
                    hablar = true;
                }
                if (cercanos.Count == 2)
                {
                    agent.destination = gameObject.transform.position;
                }
                if (visibles.Count == 0 && agent.destination == gameObject.transform.position) { estado = Estados.patrullando; }
                break;

            case Estados.esperar:
                if (cercanos.Count == 0 && visibles.Count == 0) { estado = Estados.patrullando; }
                break;

        }
    }
}

