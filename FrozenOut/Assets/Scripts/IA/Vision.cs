using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Level.Player;

public class Vision : MonoBehaviour
{

    public float RadioVista;
    public float RadioCercanos;
    public float trueSightRadius;
    [Range(0, 360)]
    public float viewAngle;

    public int TiempoDeteccion = 255;

    public LayerMask Detectable;
    public LayerMask Obstaculos;

    [HideInInspector]
    public List<Transform> ObjetosVistos = new List<Transform>();
    [HideInInspector]
    public List<Transform> ObjetosCercanos = new List<Transform>();
    [HideInInspector]
    public List<Transform> ObjetosDetectados = new List<Transform>();
    [HideInInspector]
    public List<Transform> UltimasPosiciones = new List<Transform>();
    [HideInInspector]
    public bool NoVisto = true;

    public GameObject camera;
    public GameObject DeteccionUI;
    public GameObject DeteccionUI2;
    private SpriteRenderer DeteccionSprite;
    private Renderer UIRenderer;
    private MaterialPropertyBlock _propBlock;
    private StartDetectionUIAnimation anim;

    private bool CR_running = false;
    int Deteccion = 0;



    // Start is called before the first frame update
    void Start()
    {
        ObjetosVistos.Clear();
        ObjetosCercanos.Clear();
        StartCoroutine("FindTargetsWithDelay", .2f);
        DeteccionSprite = DeteccionUI.GetComponent<SpriteRenderer>();
        DeteccionSprite.enabled = false;
        UIRenderer = DeteccionUI.GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        anim = GetComponentInChildren<StartDetectionUIAnimation>();
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
        //ObjetosCercanos.Clear();
        Collider[] colisionObjetosVistos = Physics.OverlapSphere(transform.position, RadioVista, Detectable);
        Collider[] colisionObjetosCercanos = Physics.OverlapSphere(transform.position, RadioCercanos, Detectable);
        Collider[] colisionObjetosTrueSight = Physics.OverlapSphere(transform.position, trueSightRadius, Detectable);

        //Debug.Log(colisionObjetosVistos[0]);

        for (int i = 0; i < colisionObjetosCercanos.Length; i++)
        {
            Transform target = colisionObjetosCercanos[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            //ObjetosCercanos.Add(target);

            if (target.gameObject.GetComponent<NormalController>().inStealth && Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {

                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, Obstaculos))
                {
                    ObjetosVistos.Add(target);
                }
            }
            else {
                ObjetosVistos.Add(target);
            }
        }

        for (int i = 0; i < colisionObjetosTrueSight.Length; i++)
        {
            Transform target = colisionObjetosTrueSight[i].transform;

            ObjetosDetectados.Add(target);
            ObjetosCercanos.Add(target);
            DeteccionSprite.enabled = true;
            UIRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_Change", 255);
            UIRenderer.SetPropertyBlock(_propBlock);
            Deteccion = 255;
        }

        for (int i = 0; i < colisionObjetosVistos.Length; i++)
        {

            Transform target = colisionObjetosVistos[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {

                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, Obstaculos))
                {
                    ObjetosVistos.Add(target);
                }
            }
        }
        foreach (Transform t in ObjetosVistos)
        {
            if (!CR_running && Deteccion < TiempoDeteccion)
            {
                CR_running = true;
                NoVisto = false;
                StartDetection(t);
            }
            else if (!CR_running && Deteccion >= TiempoDeteccion)
            {
                Detected(t);
            }

        }
    }

    private void Detected(Transform t)
    {
        if (!ObjetosVistos.Contains(ObjetosDetectados[0]))
        {
            UltimasPosiciones.Add(t);
            StartCoroutine(EndDetection(t));
            NoVisto = true;
        }
        else
        {
            NoVisto = false;
            UIRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_Change", 1 - ((255f - Deteccion) / 255f));
            UIRenderer.SetPropertyBlock(_propBlock);
            Deteccion = 255;
        }
    }

    private void StartDetection(Transform t)
    {
        //Debug.Log("StartDetection");
        DeteccionSprite.enabled = true; 
        //ShowUI();
        StartCoroutine(ContinueDetection(t));
    }

    private IEnumerator ContinueDetection(Transform t) {
        Deteccion++;
        UIRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_Change", 1 - ((255f - Deteccion)/255f));
        UIRenderer.SetPropertyBlock(_propBlock);
        if (Deteccion >= TiempoDeteccion)
        {
            if (!ObjetosDetectados.Contains(t))
            {
                ObjetosDetectados.Add(t);
            }
            yield return null;
            anim.StartAnimation();
            CR_running = false;
        }
        else if (ObjetosVistos.Contains(t))
        {
            yield return new WaitForSeconds(0.008f);
            StartCoroutine(ContinueDetection(t));
        } else
        {
            yield return StartCoroutine(EndDetection(t));
        }
    }

    private IEnumerator EndDetection(Transform t)
    {
        anim.StopAnimation();
        UIRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_Change", 1 - ((255f - Deteccion) / 255f));
        UIRenderer.SetPropertyBlock(_propBlock);
        if (Deteccion <= 0) {
            DeteccionSprite.enabled = false;
            ObjetosDetectados.Clear();
            UltimasPosiciones.Clear();
            NoVisto = true;
            //HideUI();
            yield return null;
            CR_running = false;
        }
        else
        {
            Deteccion--;
            if (ObjetosVistos.Contains(t))
            {
                yield return StartCoroutine(ContinueDetection(t));
            }
            else
            {
                yield return new WaitForSeconds(0.008f);
                StartCoroutine(EndDetection(t));
                
            }
        }
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        //calcular direccion desde angulo
        if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y; }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }

    private void Update()
    {
        DeteccionUI.transform.LookAt(camera.transform);
        DeteccionUI2.transform.LookAt(camera.transform);
    }
}
