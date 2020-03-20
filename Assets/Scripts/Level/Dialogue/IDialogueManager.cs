using System.Collections;
using System.Collections.Generic;

namespace Scripts.Level.Dialogue
{
    public class IDialogueManager
    {

        public bool IsRunning();
        public bool GetBoolVariable(string variableName, bool includeLeading);
        public string GetStringVariable(string variableName, bool includeLeading);
        public float GetNumberVariable(string variableName, bool includeLeading);
        public void SetVariable<T>(string variableName, T value, bool includeLeading);
        // TODO
    }
}