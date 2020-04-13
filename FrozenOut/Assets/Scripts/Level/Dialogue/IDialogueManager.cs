using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public interface IDialogueManager
    {
        bool IsRunning();
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

        event EventHandler Started;
        event EventHandler Stopped;
        event EventHandler Completed, Ended;
    }
}