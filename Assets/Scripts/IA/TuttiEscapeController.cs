using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Dialogue;

public class TuttiEscapeController : MonoBehaviour
{
    public Transform firstDestino;
    public Transform secondDestino;

    
    private bool objetivo1 = false;
    private bool objetivo2 = false;
    private bool objetivo3 = false;

    private VariableStorageYarn variableStorageYarn;
    private Animator jumpAnimator;
    private NavMeshAgent agent;
    
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageYarn>();
        agent = GetComponent<NavMeshAgent>();
        jumpAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!objetivo1 && variableStorageYarn.GetBoolValue("Tutti_Escape1") == true) {
            agent.destination = firstDestino.position;
            objetivo1 = true;
        }
        else if (!objetivo2 && variableStorageYarn.GetBoolValue("Tutti_Escape2") == true )
        {
            agent.destination = secondDestino.position;
            objetivo2 = true;
        }
        else if(!objetivo3 && variableStorageYarn.GetBoolValue("Tutti_Escape3") == true )
        {
            agent.enabled = false;
            jumpAnimator.enabled = true;
            objetivo3 = true;
        }
    }
}
