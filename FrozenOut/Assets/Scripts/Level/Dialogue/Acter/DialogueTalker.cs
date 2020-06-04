using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalker : DialogueActer
    {
        private DialogueIndicator Indicator;

        protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        void Start()
        {
            Indicator = GetComponent<DialogueIndicator>();
            SetBlocking();
            SetNonAutomatic();
        }

        public override void OnStartTalk()
        {
            Indicator.HideIndicator();
            Vector3 lookTo = LevelManager.GetPlayerManager().Player.transform.position;
            lookTo.y = transform.position.y;
            transform.LookAt(lookTo);
        }

        public override void OnEndTalk()
        {
            Indicator.ShowIndicator();
        }

        public override void OnPlayerClose()
        {
            //Indicator.ShowIndicator();
        }

        public override void OnPlayerAway()
        {
            Indicator.HideIndicator();
        }

        public override void OnSelected()
        {
            Indicator.ShowIndicator();
        }

        public override void OnDeselected()
        {
            Indicator.HideIndicator();
        }
    }
}