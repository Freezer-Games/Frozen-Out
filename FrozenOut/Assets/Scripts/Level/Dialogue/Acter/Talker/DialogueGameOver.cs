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
    }
}