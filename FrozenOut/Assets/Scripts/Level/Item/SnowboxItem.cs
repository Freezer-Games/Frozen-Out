using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue;

namespace Scripts.Level.Item
{
    public class SnowboxItem : ItemUser
    {
        public Animator BoxAnimator;
        public DialogueActer UnableTalker;

        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        public override void OnPlayerCol() {}

        public override void OnPlayerExitCol() {}

        public override void OnUse()
        {
            BoxAnimator.SetTrigger("BallJump");
        }

        public override void OnUnableUse()
        {
            DialogueManager.StartDialogue(UnableTalker);
        }
    }
}
