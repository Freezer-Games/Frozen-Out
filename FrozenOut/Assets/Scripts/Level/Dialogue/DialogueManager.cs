using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Level.Dialogue.Text;
using Scripts.Level.Dialogue.Runner;

namespace Scripts.Level.Dialogue
{
    public abstract class DialogueManager : MonoBehaviour
    {
        /// <summary>
        /// Encargado de recoger las lineas de diálogo
        /// </summary>
        public DialogueSystem DialogueSystem;

        /// <summary>
        /// Encargado de dar voz a las líneas de diálogo
        /// </summary>
        public VoiceManager VoiceManager;
        /// <summary>
        /// Encargado de mostrar las líneas de diálogo
        /// </summary>
        public TextManager TextManager;

        public abstract bool IsRunning();
        public abstract bool IsReady();

        public abstract void SetLanguage();

        public abstract void StartDialogue(DialogueTalker talker);
        public abstract void StopDialogue();
        public abstract int GetTextSize();
        public abstract KeyCode GetNextDialogueKey();
        public abstract KeyCode GetInteractKey();

        public abstract void OpenTalkPrompt(DialogueTalker dialogueTalker);
        public abstract void CloseTalkPrompt();

        public abstract bool GetBoolVariable(string variableName, bool includeLeading = true);
        public abstract string GetStringVariable(string variableName, bool includeLeading = true);
        public abstract float GetNumberVariable(string variableName, bool includeLeading = true);
        public abstract void SetVariable<T>(string variableName, T value, bool includeLeading = true);

        #region Events
        public event EventHandler Started;
        public event EventHandler Ended;
        public event EventHandler<string> LineNameUpdated;
        public event EventHandler<string> LineDialogueUpdated;

        public void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        public void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }

        public void OnLineNameUpdated(string name)
        {
            LineNameUpdated?.Invoke(this, name);
        }

        public void OnLineDialogueUpdated(string dialogue)
        {
            LineDialogueUpdated?.Invoke(this, dialogue);
        }
        #endregion
    }
}