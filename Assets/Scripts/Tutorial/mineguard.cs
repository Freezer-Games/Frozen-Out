using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class mineguard : MonoBehaviour
{
    private VariableStorageYarn storageYarn;
    private DialogueRunner dialogueSystemYarn;
    private bool hablado = false;
    public string comprobador;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        storageYarn = FindObjectOfType<VariableStorageYarn>();
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        if (cercanos.Count > 0 && hablado == false && storageYarn.GetValue(comprobador) == Yarn.Value.NULL)
        {
            animator.Play("Sorpresa");
            hablado = true;
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
        }
        // Debug.Log(FindObjectOfType<Game>().ListaObjetos.Count);
    }
}
