using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    public float closeRadius;
    public float trueSightRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector]
    public List<Transform> closeTargets = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        visibleTargets.Clear();
        closeTargets.Clear();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();

        }
    }

    void FindVisibleTargets() {
        visibleTargets.Clear();
        closeTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        Collider[] targetsInCloseRadius = Physics.OverlapSphere(transform.position, closeRadius, targetMask);
        Collider[] targetsInTrueSightRadius = Physics.OverlapSphere(transform.position, trueSightRadius, targetMask);

        for (int i = 0; i < targetsInCloseRadius.Length; i++)
        {
            Transform target = targetsInCloseRadius[i].transform;
            closeTargets.Add(target);
        }

        for (int i = 0; i < targetsInTrueSightRadius.Length; i++)
        {
            Transform target = targetsInTrueSightRadius[i].transform;
            visibleTargets.Add(target);
        }

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {

            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {

                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y;  }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }
}
