using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Level.Dialogue.System
{
    public abstract class DialogueSystem : MonoBehaviour
    {
        public abstract bool IsRunning();

        public abstract void SetLanguage(Locale locale);

        public abstract void RequestNextLine();
        public abstract void Stop();
    }

    public abstract class MainDialogueSystem : DialogueSystem
    {
        public abstract void StartDialogue(DialogueActer acter);

        public abstract void RequestSelectChoice(DialogueChoice choice);

        public abstract bool GetBoolVariable(string variableName, bool includeLeading = true);
        public abstract string GetStringVariable(string variableName, bool includeLeading = true);
        public abstract float GetNumberVariable(string variableName, bool includeLeading = true);
        public abstract void SetVariable<T>(string variableName, T value, bool includeLeading = true);
    }

    public abstract class SecondaryDialogueSystem : DialogueSystem
    {
        public abstract void StartDialogue();
    }
}