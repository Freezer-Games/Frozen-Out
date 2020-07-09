using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Level.Dialogue.Text;
using Scripts.Level.Dialogue.Runner;

namespace Scripts.Level.Dialogue
{
    public abstract class DialogueManager : BaseManager
    {
        /// <summary>
        /// Sistema actual encargado de recoger las lineas de diálogo
        /// </summary>
        protected DialogueSystem DialogueSystem;

        /// <summary>
        /// Encargado de dar voz a las líneas de diálogo
        /// </summary>
        public VoiceManager VoiceManager;
        /// <summary>
        /// Encargado de mostrar las líneas de diálogo
        /// </summary>
        public TextManager TextManager;

        /// <summary>
        /// Encargado de guardar y actualizar los estilos para cada personaje
        /// </summary>
        public StylesController StylesController;

        public abstract bool IsRunning();
        public abstract bool IsReady();
        public abstract void SetLanguage();
        public abstract void StartDialogue(DialogueActer acter);
        public abstract void StartGameOverDialogue();
        public abstract void StopDialogue();

        public abstract void SwitchToInstagram(System.Action onComplete);
        public abstract void SwitchToMain();

        public abstract int GetTextSize();
        public abstract KeyCode GetNextDialogueKey();
        public abstract KeyCode GetInteractKey();
        public abstract bool IsItemInInventory(string itemVariableName);
        public abstract bool IsItemUsed(string itemVariableName);
        public abstract int QuantityOfItem(string itemVariableName);
        public abstract void PickItem(string itemVariableName, int quantity);
        public abstract void UseItem(string itemVariableName, int quantity);
        public abstract void SetNPCAnimation(string npcName, string animation);
        public abstract void SetNPCAnimationWithSimilarName(string npcName, string animation);
        public abstract void StopNPCAnimation(string npcName);
        public abstract void StopNPCAnimationWithSimilarName(string npcName);
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

        public abstract void OnOptionsStarted(IEnumerable<DialogueOption> dialogueOptions);

        public abstract void OnOptionSelected(DialogueOption option);
        #endregion
    }

    public class DialogueOption
    {
        public string Text;
        public int ID;
    }
}