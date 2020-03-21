using System.Collections;
using System.Collections.Generic;

namespace Scripts.Level.Dialogue
{
    public interface IDialogueManager
    {

        bool IsRunning();
        bool GetBoolVariable(string variableName, bool includeLeading);
        string GetStringVariable(string variableName, bool includeLeading);
        float GetNumberVariable(string variableName, bool includeLeading);
        void SetVariable<T>(string variableName, T value, bool includeLeading);
        // TODO
    }
}