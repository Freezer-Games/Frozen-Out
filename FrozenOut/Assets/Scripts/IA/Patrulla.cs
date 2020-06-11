using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrulla : MonoBehaviour
{
    public Transform[] Destinos;
    public int SiguientePunto = 0;
    private NavMeshAgent Navegacion;
    enum Estados { patrullando, perseguir, esperar }
    Estados Estado;
    bool Hablando = false;
    private Animator Animator;
    public bool PatrullaCiclica;
    public bool PatrullaEstatica;
    private bool orden = true;//true hacia arriba false hacia abajo

    // Start is called before the first frame update
    void Start()
    {
        Navegacion = gameObject.GetComponent<NavMeshAgent>();
        Navegacion.autoBraking = false;
        Estado = Estados.patrullando;
        Animator = gameObject.GetComponent<Animator>();
    }

    void GotoNextPoint()
    {
        if (Destinos.Length == 0)
            return;
        Navegacion.destination = Destinos[SiguientePunto].position;
        if (!PatrullaCiclica && SiguientePunto == Destinos.Length)
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
        List<Transform> visibles = gameObject.GetComponent<Vision>().ObjetosDetectados;
        List<Transform> cercanos = gameObject.GetComponent<Vision>().ObjetosCercanos;
        if (!PatrullaEstatica)
        {
            Patrullar(visibles, cercanos);
        }
        else
        {
            Vigilar(visibles, cercanos);
        }

    }

    void Vigilar(List<Transform> visibles, List<Transform> cercanos)
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
                else if (visibles.Count > 0 && cercanos.Count < 1)
                {
                    Animator.SetBool("isWalking", true);
                    Navegacion.destination = visibles[0].position;
                }
                else if (cercanos.Count > 0)
                {
                    Navegacion.destination = gameObject.transform.position;
                    Animator.SetTrigger("Anim_Surprise");
                    Hablando = true;
                }
                if (visibles.Count == 0 && Navegacion.pathStatus == NavMeshPathStatus.PathComplete) { Estado = Estados.patrullando; }
                break;

            case Estados.esperar:

                break;

        }
    }


    void Patrullar(List<Transform> visibles, List<Transform> cercanos)
    {
        switch (Estado)
        {
            case Estados.patrullando:
                Animator.SetBool("isWalking", true);
                if (!Navegacion.pathPending && Navegacion.remainingDistance < 0.1f)
                    if (!PatrullaCiclica && !orden )
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
                    Hablando = true;
                }
                if (visibles.Count == 0 && Navegacion.pathStatus == NavMeshPathStatus.PathComplete) { Estado = Estados.patrullando; }
                break;

            case Estados.esperar:

                break;

        }
    }
}