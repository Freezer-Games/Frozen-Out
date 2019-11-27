using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrullaCircular : MonoBehaviour
{
    public Transform center;
    float radius = 20;
    public int points;
    private NavMeshAgent agent;
    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        center.LookAt(gameObject.transform.position);
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points==0) { return; }
        int angle = 360 / points;
        radius = Vector3.Distance(gameObject.transform.position, center.transform.position);
        center.Rotate(new Vector3(0,angle,0));
        Vector3 direction = center.forward;
        Vector3 destination = new Vector3(center.position.x+radius*direction.x,center.position.y, center.position.z + radius * direction.z);
        agent.destination = destination;
        print(agent.destination);

    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
            GotoNextPoint();
    }
}
