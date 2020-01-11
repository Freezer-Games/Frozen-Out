using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Dialogue;

public class Irse : MonoBehaviour
{
    public Transform destino;
    private NavMeshAgent agent;

    private bool objetivo = false;

    [SerializeField]
    private string ValueDialog;
    VariableStorageYarn variableStorageYarn;
    
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageYarn>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (!objetivo && variableStorageYarn.GetBoolValue(ValueDialog) == true) {
            agent.destination = destino.position;
            objetivo = true;
        }
    }
}
