using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Level.Dialogue.Text;
using Scripts.Level.Dialogue.Runner;
using Scripts.Level.Item;

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
        public abstract void StartDialogue(DialogueActer acter);
        public abstract void StopDialogue();

        public abstract int GetTextSize();
        public abstract KeyCode GetNextDialogueKey();
        public abstract KeyCode GetInteractKey();
        public abstract bool IsItemInInventory(string itemVariableName);
        public abstract bool IsItemUsed(string itemVariableName);
        public abstract void PickItem(string itemVariableName, int quantity);
        public abstract void UseItem(string itemVariableName, int quantity);
        public abstract void SetNPCAnimation(string npcName, string animation);
        public abstract void StopNPCAnimation(string npcName);
        public abstract GameObject GetPlayer();

        public abstract void OpenTalkPrompt(DialogueActer dialogueActer);
        public abstract void CloseTalkPrompt(DialogueActer dialogueActer);

        public abstract bool GetBoolVariable(string variableName, bool includeLeading = true);
        public abstract string GetStringVariable(string variableName, bool includeLeading = true);
        public abstract float GetNumberVariable(string variableName, bool includeLeading = true);
        public abstract void SetVariable<T>(string variableName, T value, bool includeLeading = true);

        #region Events
        public event EventHandler Started;
        public event EventHandler Ended;

        public virtual void OnDialogueStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnDialogueEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }

        public abstract void OnLineStyleUpdated(string styleName);

        public abstract void OnLineNameUpdated(string name);

        public abstract void OnLineDialogueUpdated(string dialogue);
        #endregion
    }
}