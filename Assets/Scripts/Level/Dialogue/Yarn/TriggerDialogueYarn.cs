using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class TriggerDialogueYarn : MonoBehaviour
    {
        private IDialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        void Start()
        {
            
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("NPC"))
            {
                DialogueIndicator indicator = other.GetComponent<DialogueIndicator>();
                indicator.ShowIndicator();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("NPC") && DialogueManager.IsReady() && Input.GetKey(DialogueManager.GetInteractKey()))
            {
                DialogueTalker target = other.GetComponent<DialogueTalker>();
                DialogueIndicator indicator = other.GetComponent<DialogueIndicator>();
                indicator.HideIndicator();
                
                if(target != null)
                {
                    DialogueManager.StartDialogue(target.talkToNode);

                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("NPC"))
            {
                DialogueIndicator indicator = other.GetComponent<DialogueIndicator>();
                indicator.HideIndicator();
            }
        }

    }
}