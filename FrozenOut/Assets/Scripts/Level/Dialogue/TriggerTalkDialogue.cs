using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class TriggerTalkDialogue : MonoBehaviour
    {
        private IDialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        void Start()
        {
            
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("NPC"))
            {
                DialogueTalker targetDialogue = other.GetComponent<DialogueTalker>();
                targetDialogue.OnPlayerClose();
                
                DialogueManager.OpenTalkPrompt(targetDialogue);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("NPC"))
            {
                DialogueTalker targetDialogue = other.GetComponent<DialogueTalker>();
                targetDialogue.OnPlayerAway();
                
                DialogueManager.CloseTalkPrompt();
            }
        }

    }
}