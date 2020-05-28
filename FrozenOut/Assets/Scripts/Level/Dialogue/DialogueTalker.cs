using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalker : MonoBehaviour
    {
        public string TalkToNode = "";

        public CharacterDialogueStyle Style;
        public List<CharacterDialogueStyle> ExtraStyles;

        private DialogueIndicator Indicator;

        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        void Start()
        {
            Indicator = GetComponent<DialogueIndicator>();
        }

        public void OnStartTalk()
        {
            Indicator.HideIndicator();
            Vector3 lookTo = LevelManager.GetPlayerManager().Player.transform.position;
            lookTo.y = transform.position.y;
            transform.LookAt(lookTo);
        }

        public void OnEndTalk()
        {
            Indicator.ShowIndicator();
        }

        public void OnPlayerClose()
        {
            Indicator.ShowIndicator();
        }

        public void OnPlayerAway()
        {
            Indicator.HideIndicator();
        }

    }
}