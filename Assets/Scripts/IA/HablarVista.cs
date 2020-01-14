using UnityEngine;
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
    [System.NonSerialized]
    public bool vuelta = false;
    private Transform player;
    [SerializeField]
    private Transform puesto;
    public string comprobador;
    public string animacion;
    public string animacion2;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        storageYarn = FindObjectOfType<VariableStorageYarn>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        agent = GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
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
                animator.SetBool(animacion, true);
                animator.Play(animacion2);
                player = GameObject.Find("POL").transform;
                agent.destination = player.position;
                hablado = false;
                movido = true;
                
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance && hablado == false)
        {
            animator.SetBool(animacion, false);
            animator.Play("Respiración");
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
            hablado = true;
        }
        if (movido == true && hablado == true && !dialogueSystemYarn.isDialogueRunning && vuelta == false)
        {
            vuelta = true;
            animator.Play(animacion2);
            animator.SetBool(animacion, true);
            agent.stoppingDistance = 0.2f;
            agent.destination = puesto.position;
        }
        if (agent.isStopped) {
            animator.SetBool(animacion, false);
            animator.Play("Respiración"); 
        }
    }
}