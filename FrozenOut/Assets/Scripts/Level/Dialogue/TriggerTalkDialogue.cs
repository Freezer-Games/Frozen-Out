using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueTalker))]
    public class TriggerTalkDialogue : MonoBehaviour
    {
        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        private string PlayerTag = "Player";
        private DialogueTalker Talker;

        void Start()
        {
            Talker = GetComponent<DialogueTalker>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                Talker.OnPlayerClose();
                
                DialogueManager.OpenTalkPrompt(Talker);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                Talker.OnPlayerAway();
                
                DialogueManager.CloseTalkPrompt(Talker);
            }
        }

    }
}