using System.Collections;
using System.Collections.Generic;
using System;

namespace Scripts.Level.Dialogue
{
    public interface IDialogueManager
    {

        bool IsRunning();
        bool GetBoolVariable(string variableName, bool includeLeading = true);
        string GetStringVariable(string variableName, bool includeLeading = true);
        float GetNumberVariable(string variableName, bool includeLeading = true);
        void SetVariable<T>(string variableName, T value, bool includeLeading = true);
        // TODO

        event EventHandler Starting, Started;
        event EventHandler Stopping, Stopped;
        event EventHandler Completed, Ended;
    }
}