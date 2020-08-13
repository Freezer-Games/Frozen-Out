using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class DialogueActer : MonoBehaviour
    {
        public string StoryNode = "";
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

        protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

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
}