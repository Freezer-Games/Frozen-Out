using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class mineguard : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;
    private bool hablado = false;
    // Start is called before the first frame update
    void Start()
    {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> cercanos = gameObject.GetComponent<FieldOfView>().closeTargets;
        if (cercanos.Count > 0 && hablado == false)
        {
            hablado = true;
            dialogueSystemYarn.StartDialogue(gameObject.GetComponent<NPCYarn>().talkToNode);
        }
    }
}
