using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class Hablar : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private NavMeshAgent agent;
    public Transform player;
    private bool hablado = false;
    public Canvas tutcanvas;
    private bool tut = true;

    // Start is called before the first frame update
    void Start()
    {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.position;
        dialogueSystemYarn.isDialogueWaiting = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && hablado == false)
        {
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
            hablado = true;
            dialogueSystemYarn.isDialogueWaiting = false;
        }
        if (!dialogueSystemYarn.isDialogueWaiting && !dialogueSystemYarn.isDialogueRunning && tut)
        {
            tutcanvas.enabled = true;
            tut = false;
        }
    }
}