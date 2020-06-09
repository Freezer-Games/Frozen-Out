using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueGameOver : DialogueTalker
    {
        public override void OnStartTalk()
        {
            
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