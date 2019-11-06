using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueController : MonoBehaviour
{
    private DialogueRunner dialogueSystemYarn;

    // Start is called before the first frame update
    void Start()
    {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueSystemYarn.isDialogueRunning == true) {
            return;
        }
    }
}
