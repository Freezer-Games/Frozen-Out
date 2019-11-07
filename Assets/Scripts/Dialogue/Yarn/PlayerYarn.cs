﻿using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PlayerYarn : MonoBehaviour
{
    public KeyCode dialogueInput = KeyCode.F;
    public Text dialoguePromptText;

    private DialogueRunner dialogueSystemYarn;

    /// Draw the range at which we'll start talking to people.
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
    }

    void Start() {
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        dialoguePromptText.text = "";

        PlayerController controller = FindObjectOfType<PlayerController>();
        controller.Moving += Player_Moving;
        controller.Idle += Player_Idle;
    }

    private void Player_Idle(object sender, System.EventArgs e)
    {
        this.enabled = true;
    }

    private void Player_Moving(object sender, PlayerControllerEventArgs e)
    {
        if (dialogueSystemYarn.isDialogueRunning) e.Cancel = true;
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "NPC")
        {
            dialoguePromptText.text = "Press [" + dialogueInput.ToString() + "] to talk";
        }
    }

    private void OnTriggerStay(Collider other) {
        
        if (this.isActiveAndEnabled && !dialogueSystemYarn.isDialogueRunning && (other.gameObject.tag == "NPC") && Input.GetKeyDown(dialogueInput))
        {
            dialoguePromptText.text = "";
            NPCYarn target = other.gameObject.GetComponent<NPCYarn>();
            if(target != null) {
                dialogueSystemYarn.StartDialogue (target.talkToNode);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "NPC")
        {
            dialoguePromptText.text = "";
        }
    }
}
