using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public interface IDialogueManager
    {

        event EventHandler<DialogueStartingEventArgs> Starting;
        event EventHandler<DialogueEventArgs> Started;
        event EventHandler Stopping, Stopped;
        event EventHandler Completed, Ended;

        bool IsRunning();
        bool IsStarting();
        bool IsReady();

        void SetLanguage();

        void StartDialogue(string startNode);
        KeyCode GetInteractKey();
        
        bool GetBoolVariable(string variableName, bool includeLeading = true);
        string GetStringVariable(string variableName, bool includeLeading = true);
        float GetNumberVariable(string variableName, bool includeLeading = true);
        void SetVariable<T>(string variableName, T value, bool includeLeading = true);
    }

    [Serializable]
    public class DialogueEventArgs : EventArgs
    {

        public string StartNode
        {
            get;
        }

        public DialogueEventArgs(string startNode)
        {
            this.StartNode = startNode;
        }

    }

    [Serializable]
    public class DialogueStartingEventArgs : DialogueEventArgs
    {

        public event EventHandler Canceling;

        public void Cancel()
        {
            Canceling?.Invoke(this, EventArgs.Empty);
        }

        public DialogueStartingEventArgs(string startNode) : base(startNode)
        {
            
        }

    }
}