using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueActer))]
    public abstract class TriggerActDialogue : TriggerBase
    {
        protected DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

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

        public void StartTalk()
        {
            DialogueManager.StartDialogue(Acter);
        }
    }

    public class TriggerTalkDialogue : TriggerActDialogue
    {
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