using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class perseguir : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        Debug.Log("visibles:"+visibles.Count);
        Debug.Log("cercanos:" + cercanos.Count);
        if (visibles.Count>0 && cercanos.Count<2) { agent.destination = visibles[0].position; }
    }
}
