using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class DialogueManager : MonoBehaviour
    {
        public abstract bool IsRunning();
        public abstract bool IsReady();

        public abstract void SetLanguage();

        public abstract void StartDialogue(DialogueTalker talker);
        public abstract void StopDialogue();
        public abstract void OpenTalkPrompt(DialogueTalker talker);
        public abstract void CloseTalkPrompt();
        
        public abstract bool GetBoolVariable(string variableName, bool includeLeading = true);
        public abstract string GetStringVariable(string variableName, bool includeLeading = true);
        public abstract float GetNumberVariable(string variableName, bool includeLeading = true);
        public abstract void SetVariable<T>(string variableName, T value, bool includeLeading = true);

        #region Events
        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Completed, Ended;

        protected void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        protected void OnStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        protected void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        protected void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}