using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts;
using Scripts.Level;
using Scripts.Level.Dialogue;

public class Patrulla : MonoBehaviour
{

    protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;
    protected DialogueActer Acter;
    private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

    private NavMeshAgent Navegacion;
    enum Estados { patrullando, perseguir, esperar, regresar }
    Estados Estado;
    enum TipoPatrulla { Cíclica, VueltaAtras, Estatica }
    [SerializeField]
    
    TipoPatrulla Tipo;

    public Transform[] Destinos;
    public Transform PosicionPatrulla;
    public Transform Mira;
    public int SiguientePunto = 0;

    private bool Hablando = false;
    private Animator Animator;
    private bool orden = true; //true upwards false hacdownwards
    private bool NoVisto;

    bool pausado;
    public bool TienePausas;
    public Transform[] Pausas;
    public Transform[] MiraPausas;
    public float[] TiemposPausas;

    void Start()
    {
        Navegacion = gameObject.GetComponent<NavMeshAgent>();
        Navegacion.autoBraking = false;
        Estado = Estados.patrullando;
        Animator = gameObject.GetComponent<Animator>();
        Acter = GetComponent<DialogueActer>();
        if (Tipo == TipoPatrulla.Estatica)
        gameObject.transform.LookAt(Mira);
    }

    void GotoNextPoint()
    {
        if (Destinos.Length == 0)
            return;
        Navegacion.destination = Destinos[SiguientePunto].position;
        if (Tipo != TipoPatrulla.Cíclica && SiguientePunto == Destinos.Length)
        {
            SiguientePunto = (SiguientePunto - 1);
            orden = false;
        }
        else
        {
            SiguientePunto = (SiguientePunto + 1) % Destinos.Length;
        }

    }

    void GotoPreviousPoint()
    {
        if (Destinos.Length == 0)
            return;
        Navegacion.destination = Destinos[SiguientePunto].position;
        if (SiguientePunto == 0)
        {
            SiguientePunto = (SiguientePunto + 1) % Destinos.Length;
            orden = true;
        }
        else
        {
            SiguientePunto = (SiguientePunto - 1);
        }
        SiguientePunto = (SiguientePunto + 1) % Destinos.Length;
    }

    void Update()
    {
        bool shouldMove = (Navegacion.remainingDistance <= Navegacion.stoppingDistance && Navegacion.velocity.magnitude < 0.1f);
        if (shouldMove)
        {
            Animator.SetBool("isWalking", false);
        }
        else
        {
            Animator.SetBool("isWalking", true);
        }
        NoVisto = gameObject.GetComponent<Vision>().NoVisto;
        List<Transform> visibles = gameObject.GetComponent<Vision>().ObjetosDetectados;
        List<Transform> cercanos = gameObject.GetComponent<Vision>().ObjetosCercanos;
        List<Transform> ultimasPosiciones = gameObject.GetComponent<Vision>().UltimasPosiciones;
        if (!pausado)
        {
            if (Tipo != TipoPatrulla.Estatica)
            {
                Patrullar(visibles, cercanos, ultimasPosiciones);
            }
            else
            {
                Vigilar(visibles, cercanos, ultimasPosiciones);
            }
        }

    }

    void Vigilar(List<Transform> visibles, List<Transform> cercanos, List<Transform> ultimasPosiciones)
    {
        switch (Estado)
        {
            case Estados.patrullando:
                gameObject.transform.LookAt(Mira);
                if (visibles.Count > 0)
                {
                    Animator.SetTrigger("Anim_Surprise");
                    Estado = Estados.perseguir;
                }
                break;

            case Estados.perseguir:
                if (Hablando)
                {
                    IniciarDialogo();
                    Estado = Estados.esperar;
                }
                else if (visibles.Count > 0 && cercanos.Count < 1 && NoVisto)
                {
                    Navegacion.destination = ultimasPosiciones[0].position;
                }
                else if (visibles.Count > 0 && cercanos.Count < 1 && !NoVisto)
                {
                    Navegacion.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0)
                {
                    Navegacion.destination = gameObject.transform.position;
                    Hablando = true;
                }
                if (visibles.Count == 0 && Navegacion.pathStatus == NavMeshPathStatus.PathComplete) { Estado = Estados.regresar; }
                break;

            case Estados.regresar:
                Navegacion.destination = PosicionPatrulla.position;

                if (Navegacion.remainingDistance <= Navegacion.stoppingDistance)
                {
                    if (!Navegacion.hasPath || Navegacion.velocity.sqrMagnitude == 0f)
                    {
                        Estado = Estados.patrullando;
                    }
                }
                break;

            case Estados.esperar:

                break;

        }
    }


    void Patrullar(List<Transform> visibles, List<Transform> cercanos, List<Transform> ultimasPosiciones)
    {
        switch (Estado)
        {
            case Estados.patrullando:
                if (!Navegacion.pathPending && Navegacion.remainingDistance < 0.35f)
                    if (Tipo != TipoPatrulla.Cíclica && !orden )
                    {
                        StartCoroutine(Esperar(2));
                    }
                    else 
                    {
                        StartCoroutine(Esperar(1));
                    }
                if (visibles.Count > 0 )
                {
                    Animator.Play("Sorpresa");
                    Estado = Estados.perseguir;
                }
                break;

            case Estados.perseguir:
                if (Hablando)
                {
                    IniciarDialogo();
                    Estado = Estados.esperar;
                }
                else if (visibles.Count > 0 && cercanos.Count < 1)
                {
                    Navegacion.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0 )
                {
                    Navegacion.destination = gameObject.transform.position;
                    Hablando = true;
                }
                if (visibles.Count == 0 && Navegacion.pathStatus == NavMeshPathStatus.PathComplete) { Estado = Estados.patrullando; }
                break;

            case Estados.esperar:

                break;

        }
    }

    private void IniciarDialogo()
    {
        Vector3 lookTo = LevelManager.GetPlayerManager().Player.transform.position;
        lookTo.y = transform.position.y;
        transform.LookAt(lookTo);

        DialogueManager.StartGameOverDialogue();
    }

    private IEnumerator Esperar(int tipo)//1 for NextPoint, 2 for PreviousPoint
    {
        if (TienePausas)
        {
            for (int i = 0; i < Pausas.Length; i++)
            {
                if (Vector3.Distance(Navegacion.destination, Pausas[i].position) < 0.1)
                {
                    transform.LookAt(MiraPausas[i]);
                    pausado = true;
                    Navegacion.destination = gameObject.transform.position;
                    yield return new WaitForSeconds(TiemposPausas[i]);
                    pausado = false;
                }
            }
        }
        if (tipo == 1) { GotoNextPoint(); }
        else { GotoPreviousPoint(); }
    }
}