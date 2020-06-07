using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Level.Dialogue.Runner
{
    public abstract class DialogueSystem : MonoBehaviour
    {
        public abstract bool IsRunning();

        public abstract void SetLanguage(Locale locale);

        public abstract void StartDialogue(DialogueActer acter);
        public abstract void RequestNextLine();
        public abstract void Stop();

        public abstract bool GetBoolVariable(string variableName, bool includeLeading = true);
        public abstract string GetStringVariable(string variableName, bool includeLeading = true);
        public abstract float GetNumberVariable(string variableName, bool includeLeading = true);
        public abstract void SetVariable<T>(string variableName, T value, bool includeLeading = true);
    }
}