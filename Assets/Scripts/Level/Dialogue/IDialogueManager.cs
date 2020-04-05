using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public interface IDialogueManager
    {
        bool IsRunning();
        bool IsStarting();
        bool IsReady();

        void SetLanguage();

        void StartDialogue(DialogueTalker talker);
        void StopDialogue();
        void OpenTalkPrompt(DialogueTalker dialogueTalker);
        void CloseTalkPrompt();
        
        bool GetBoolVariable(string variableName, bool includeLeading = true);
        string GetStringVariable(string variableName, bool includeLeading = true);
        float GetNumberVariable(string variableName, bool includeLeading = true);
        void SetVariable<T>(string variableName, T value, bool includeLeading = true);

        event EventHandler<DialogueStartingEventArgs> Starting;
        event EventHandler<DialogueEventArgs> Started;
        event EventHandler Stopping, Stopped;
        event EventHandler Completed, Ended;
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