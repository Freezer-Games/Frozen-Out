using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;

public class PlayerYarn : MonoBehaviour
{
    public KeyCode DialogueInput = KeyCode.F;

    private DialogueRunner dialogueSystemYarn;

    /// Draw the range at which we'll start talking to people.
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
    }

    void Start() {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
    }

    /// Update is called once per frame
    void Update () {

        // Remove all player control when we're in dialogue
        if (dialogueSystemYarn.isDialogueRunning == true) {
            return;
        }

    }

    private void OnTriggerStay(Collider other) {
        
        if (!dialogueSystemYarn.isDialogueRunning && (other.gameObject.tag == "NPC") && Input.GetKeyDown(DialogueInput))
        {
            NPCYarn target = other.gameObject.GetComponent<NPCYarn>();
            if(target != null) {
                dialogueSystemYarn.StartDialogue (target.talkToNode);
            }
        }
    }
}
