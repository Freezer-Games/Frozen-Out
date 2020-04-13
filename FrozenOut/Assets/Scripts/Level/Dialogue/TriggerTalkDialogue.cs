using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueTalker))]
    public class TriggerTalkDialogue : MonoBehaviour
    {
        private IDialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        private String PLayerTag = "Player";
        private DialogueTalker Talker;

        void Start()
        {
            Talker = GetComponent<DialogueTalker>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(PLayerTag))
            {
                Talker.OnPlayerClose();
                
                DialogueManager.OpenTalkPrompt(Talker);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(PLayerTag))
            {
                Talker.OnPlayerAway();
                
                DialogueManager.CloseTalkPrompt();
            }
        }

    }
}