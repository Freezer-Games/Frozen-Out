using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class YarnDialoguePromptController : MonoBehaviour
    {

        public YarnManager DialogueManager;
        
        private bool IsOpen => CandidateTalker != null;

        private DialogueTalker CandidateTalker;

        void Update()
        {
            if(IsOpen && DialogueManager.IsReady() && Input.GetKey(DialogueManager.GetInteractKey()))
            {
                DialogueManager.StartDialogue(CandidateTalker);
            }
        }

        public void Open(DialogueTalker talker)
        {
            CandidateTalker = talker;
            // Enable potential canvas
        }

        public void Close()
        {
            CandidateTalker = null;
            // Disable potential canvas
        }

    }
}