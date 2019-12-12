using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class irse1 : MonoBehaviour
{
    public Transform destino;
    private NavMeshAgent agent;
    private bool objetivo = false;
    [SerializeField]
    private string ValueDialog;
    VariableStorageBehaviour variableStorageYarn;
    // Start is called before the first frame update
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageBehaviour>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (variableStorageYarn.GetValue(ValueDialog) != Yarn.Value.NULL && !objetivo) {
            agent.destination = destino.position;
            objetivo = true;
        }
    }
}
