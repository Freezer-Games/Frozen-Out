using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueActer))]
    public abstract class TriggerActDialogue : MonoBehaviour
    {
        private readonly string PlayerTag = "Player";
        protected DialogueActer Acter;

        void Start()
        {
            Acter = GetComponent<DialogueActer>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                OnPlayerEnter();
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                OnPlayerExit();
            }
        }

        protected virtual void OnPlayerEnter()
        {
            Acter.OnPlayerClose();
        }

        protected virtual void OnPlayerExit()
        {
            Acter.OnPlayerAway();
        }
    }

    public class TriggerTalkDialogue : TriggerActDialogue
    {
        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        protected override void OnPlayerEnter()
        {
            base.OnPlayerEnter();
            
            DialogueManager.OpenTalkPrompt(Acter);
        }

        protected override void OnPlayerExit()
        {
            base.OnPlayerExit();
            
            DialogueManager.CloseTalkPrompt(Acter);
        }
    }
}