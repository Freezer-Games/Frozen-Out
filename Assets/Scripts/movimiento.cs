using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class movimiento : MonoBehaviour
{
    VariableStorageBehaviour variableStorageYarn;
    public Transform goal;
    private bool objetivo = false;

    // Start is called before the first frame update
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (variableStorageYarn.GetValue("$talked_to_moving") != Yarn.Value.NULL && objetivo==false)
        {

            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = goal.position;
            objetivo = true;
        }
    }
}
