using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class perseguir : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        List<Transform> visibles = gameObject.GetComponent<FieldOfView>().visibleTargets;
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        if (visibles.Count==1 && cercanos.Count==0) { agent.destination = visibles[0].position; }
    }
}
