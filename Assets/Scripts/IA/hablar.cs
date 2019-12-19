using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Assets.Scripts.Dialogue;
using System.Collections;

public class hablar : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private NavMeshAgent agent;
    private bool hablado = true;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (Game.instance.loading == false)
        {
            player = GameObject.Find("POL").transform;
            dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
            hablado = !(GameObject.FindObjectOfType<MissionsCanvas>().Missions.Count == 0);
            agent = GetComponent<NavMeshAgent>();
            agent.destination = gameObject.transform.position - new Vector3(0, 0, agent.stoppingDistance);
            if (hablado == false)
            {
                agent.destination = player.position;
            }
        }
    
    }





    // Update is called once per frame
    void Update()
    {
        if (hablado == false)
        {

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
                hablado = true;
            }
        }
    }
}