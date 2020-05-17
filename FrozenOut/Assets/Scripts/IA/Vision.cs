using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

    public float RadioVista;
    public float RadioCercanos;
    public float trueSightRadius;
    [Range(0, 360)]
    public float viewAngle;

    public int TiempoDeteccion = 255;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> ObjetosVistos = new List<Transform>();
    [HideInInspector]
    public List<Transform> ObjetosCercanos = new List<Transform>();
    [HideInInspector]
    public List<Transform> ObjetosDetectados = new List<Transform>();

    private bool CR_running = false;
    int deteccion = 0;


    // Start is called before the first frame update
    void Start()
    {
        ObjetosVistos.Clear();
        ObjetosCercanos.Clear();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();

        }
    }

    void FindVisibleTargets()
    {
        //vaciamos listas y comprobamos las vistas con esferea de vision
        ObjetosVistos.Clear();
        ObjetosCercanos.Clear();
        Collider[] colisionObjetosVistos = Physics.OverlapSphere(transform.position, RadioVista, targetMask);
        Collider[] colisionObjetosCercanos = Physics.OverlapSphere(transform.position, RadioCercanos, targetMask);
        Collider[] colisionObjetosTrueSight = Physics.OverlapSphere(transform.position, trueSightRadius, targetMask);

        //Debug.Log(colisionObjetosVistos[0]);

        for (int i = 0; i < colisionObjetosCercanos.Length; i++)
        {
            Transform target = colisionObjetosCercanos[i].transform;
            ObjetosCercanos.Add(target);
        }

        for (int i = 0; i < colisionObjetosTrueSight.Length; i++)
        {
            Transform target = colisionObjetosTrueSight[i].transform;
            ObjetosVistos.Add(target);
        }

        for (int i = 0; i < colisionObjetosVistos.Length; i++)
        {

            Transform target = colisionObjetosVistos[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {

                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    ObjetosVistos.Add(target);
                }
            }
        }
        foreach (Transform t in ObjetosVistos)
        {
            if (!CR_running && deteccion < TiempoDeteccion)
                StartCoroutine(StartDetection(t)); 
        }
    }

    private IEnumerator StartDetection(Transform t) {
        CR_running = true;
        Debug.Log("StartDetection");
        deteccion = deteccion + 1;
        gameObject.GetComponentInChildren<DetectionUI>().UpdateUI(deteccion);

        if (deteccion >= TiempoDeteccion)
        {
            ObjetosDetectados.Add(t);
            yield return null;
            CR_running = false;
        }
        else if (ObjetosVistos.Contains(t))
        {
            yield return new WaitForSeconds(0.005f);
            StartCoroutine(StartDetection(t));
        } else
        {
            StartCoroutine(EndDetection(t));
            yield return null;
        }
    }

    private IEnumerator EndDetection(Transform t)
    {

        deteccion = deteccion - 1;
        gameObject.GetComponentInChildren<DetectionUI>().UpdateUI(deteccion);

        if (ObjetosVistos.Contains(t))
        {
            StartCoroutine(StartDetection(t));
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.005f);
            CR_running = false;
        }
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        //calcular direccion desde angulo
        if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y; }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }

    
}
