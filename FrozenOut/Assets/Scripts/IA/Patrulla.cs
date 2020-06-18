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
    enum Estados { patrullando, perseguir, esperar }
    Estados Estado;
    enum TipoPatrulla { Cíclica, VueltaAtras , Estatica }
    [SerializeField]
    TipoPatrulla Tipo;

    public Transform[] Destinos;
    public int SiguientePunto = 0;

    private bool Hablando = false;
    private Animator Animator;
    private bool orden = true;//true hacia arriba false hacia abajo
    private bool NoVisto;

    // Start is called before the first frame update
    void Start()
    {
        Navegacion = gameObject.GetComponent<NavMeshAgent>();
        Navegacion.autoBraking = false;
        Estado = Estados.patrullando;
        Animator = gameObject.GetComponent<Animator>();
        Acter = GetComponent<DialogueActer>();
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

    // Update is called once per frame
    void Update()
    {
        NoVisto = gameObject.GetComponent<Vision>().NoVisto;
        List<Transform> visibles = gameObject.GetComponent<Vision>().ObjetosDetectados;
        List<Transform> cercanos = gameObject.GetComponent<Vision>().ObjetosCercanos;
        List<Transform> ultimasPosiciones = gameObject.GetComponent<Vision>().UltimasPosiciones;
        if (Tipo != TipoPatrulla.Estatica)
        {
            Patrullar(visibles, cercanos, ultimasPosiciones);
        }
        else
        {
            Vigilar(visibles, cercanos, ultimasPosiciones);
        }

    }

    void Vigilar(List<Transform> visibles, List<Transform> cercanos, List<Transform> ultimasPosiciones)
    {
        switch (Estado)
        {
            case Estados.patrullando:
                Animator.SetBool("isWalking", false);
                if (visibles.Count > 0)
                {
                    Animator.SetTrigger("Anim_Surprise");
                    Estado = Estados.perseguir;
                }
                break;

            case Estados.perseguir:
                if (Hablando)
                {
                    Animator.SetBool("isWalking", false);
                    //Comenzar dialogo
                    Estado = Estados.esperar;
                }
                else if (visibles.Count > 0 && cercanos.Count < 1 && NoVisto)
                {
                    Animator.SetBool("isWalking", true);
                    Navegacion.destination = ultimasPosiciones[0].position;
                }
                else if (visibles.Count > 0 && cercanos.Count < 1 && !NoVisto)
                {
                    Animator.SetBool("isWalking", true);
                    Navegacion.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0)
                {
                    Navegacion.destination = gameObject.transform.position;
                    Animator.SetTrigger("Anim_Surprise");
                    IniciarDialogo();
                    Hablando = true;
                }
                if (visibles.Count == 0 && Navegacion.pathStatus == NavMeshPathStatus.PathComplete) { Estado = Estados.patrullando; }
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
                Animator.SetBool("isWalking", true);
                if (!Navegacion.pathPending && Navegacion.remainingDistance < 0.1f)
                    if (Tipo != TipoPatrulla.Cíclica && !orden )
                    {
                        GotoPreviousPoint();
                    }
                    else 
                    {
                        GotoNextPoint();
                    }
                if (visibles.Count > 0 )
                {
                    Animator.SetBool("isWalking", false);
                    Animator.Play("Sorpresa");
                    Estado = Estados.perseguir;
                }
                break;

            case Estados.perseguir:
                Debug.Log("Cercanos = " + cercanos.Count);
                if (Hablando)
                {
                    Animator.SetBool("isWalking", false);
                    //Comenzar dialogo
                    Estado = Estados.esperar;
                }
                else if (visibles.Count > 0 && cercanos.Count < 1)
                {
                    Animator.SetBool("isWalking", true);
                    Navegacion.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0 )
                {
                    Navegacion.destination = gameObject.transform.position;
                    Animator.SetTrigger("Anim_Surprise");
                    IniciarDialogo();
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

        DialogueManager.OpenTalkPrompt(Acter);
    }
}