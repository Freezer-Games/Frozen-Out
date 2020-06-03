using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class DialogueActer : MonoBehaviour
    {
        public string TalkToNode = "";
        public bool IsBlocking
        {
            get;
            private set;
        }
        public bool IsAutomatic
        {
            get;
            private set;
        }

        public CharacterDialogueStyle Style;
        public List<CharacterDialogueStyle> ExtraStyles;

        public void SetBlocking()
        {
            this.IsBlocking = true;
        }

        public void SetNonBlocking()
        {
            this.IsBlocking = false;
        }

        public void SetAutomatic()
        {
            this.IsAutomatic = true;
        }

        public void SetNonAutomatic()
        {
            this.IsAutomatic = false;
        }

        public abstract void OnStartTalk();
        public abstract void OnEndTalk();

        public abstract void OnPlayerClose();
        public abstract void OnPlayerAway();

        public abstract void OnSelected();
        public abstract void OnDeselected();
    }

    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalker : DialogueActer
    {
        private DialogueIndicator Indicator;

        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

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
            //Indicator.ShowIndicator();
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