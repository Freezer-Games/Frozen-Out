using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class Irse : MonoBehaviour
{
    public Transform destino;
    private NavMeshAgent agent;

    private bool objetivo = false;

    [SerializeField]
    private string ValueDialog;
    VariableStorageBehaviour variableStorageYarn;
    
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageBehaviour>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (!objetivo && variableStorageYarn.GetValue(ValueDialog) != Yarn.Value.NULL) {
            agent.destination = destino.position;
            objetivo = true;
        }
    }
}
