using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueGameOver : DialogueTalker
    {
        public DialogueManager DialogueManager;

        public override void OnStartTalk()
        {
            DialogueManager.Disable();
        }

        public override void OnEndTalk()
        {
            LevelManager.GameOver();
        }

        public override void OnPlayerClose()
        {
            
        }

        public override void OnPlayerAway()
        {
            
        }

        public override void OnSelected()
        {
            
        }

        public override void OnDeselected()
        {
            
        }
    }
}