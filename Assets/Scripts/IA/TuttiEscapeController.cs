using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Dialogue;

public class TuttiEscapeController : MonoBehaviour
{
    public Transform firstDestino;
    public Transform secondDestino;
	
	public GameObject tuttiSinNavPrefab;
    
    private bool objetivo1 = false;
    private bool objetivo2 = false;
    private bool objetivo3 = false;

    private VariableStorageYarn variableStorageYarn;
    private Animator tuttiAnimator;
    private NavMeshAgent agent;
    
    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageYarn>();
        agent = GetComponent<NavMeshAgent>();
        tuttiAnimator = GetComponent<Animator>();
		agent.updatePosition = false;
    }
    
    void Update()
    {
		if(transform.position == firstDestino.position || transform.position == secondDestino.position) {
			GameManager.instance.NormalMode();
            tuttiAnimator.SetBool("isMoving", false);
		}
		if(objetivo3) {
			agent.destination = transform.position;
		}
		
        if (!objetivo1 && variableStorageYarn.GetBoolValue("Tutti_Escape1") == true) {
			tuttiAnimator.SetBool("isMoving", true);
			tuttiAnimator.Play("Caminado_Anticipacion");
            agent.destination = firstDestino.position;
            objetivo1 = true;
        }
        else if (!objetivo2 && variableStorageYarn.GetBoolValue("Tutti_Escape2") == true )
        {
			tuttiAnimator.SetBool("isMoving", true);
			tuttiAnimator.Play("Caminado_Anticipacion");
            agent.destination = secondDestino.position;
            objetivo2 = true;
        }
        else if(!objetivo3 && variableStorageYarn.GetBoolValue("Tutti_Escape3") == true )
        {
			/*tuttiAnimator.SetTrigger("escapeSalto");
			tuttiAnimator.Play("Tutti_Escapando");*/
			GameObject.Instantiate(tuttiSinNavPrefab);
			Destroy(gameObject);
        }
    }
}
