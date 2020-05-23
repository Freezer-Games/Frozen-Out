using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Dialogue.Text.Unity
{
    [RequireComponent(typeof(Canvas))]
    public class UnityTextManager : TextManager
    {
        public Canvas DialogueCanvas;

        private bool IsOpen => DialogueCanvas.enabled;

        private string CurrentSentence;
        private TextStyle CurrentStyle;

        void Awake()
        {
            Close();
        }

        public override void Open()
        {
            DialogueCanvas.enabled = true;
        }

        public override void Close()
        {
            DialogueCanvas.enabled = false;
        }

        public override void SetStyle(TextStyle style)
        {
            CurrentStyle = style;
            OnStyleLineUpdate(style);
        }

        public override void ShowName(string name)
        {
            OnNameLineUpdate(name);
        }

        public override void ShowName(char newNameLetter)
        {
            throw new System.NotImplementedException();
        }

        public override void ShowDialogue(string dialogue)
        {
            // TODO with textstyle
            OnDialogueLineUpdate(dialogue);
        }

        public override void ShowDialogue(char newDialogLetter)
        {
            throw new System.NotImplementedException();
        }

        #region Events
        public StringUnityEvent LineNameUpdated;
        public StringUnityEvent LineDialogueUpdated;
        public StyleUnityEvent LineStyleUpdated;

        private void OnNameLineUpdate(string nameToDisplay)
        {
            LineNameUpdated?.Invoke(nameToDisplay);
        }

        private void OnDialogueLineUpdate(string dialogueToDisplay)
        {
            LineDialogueUpdated?.Invoke(dialogueToDisplay);
        }

        private void OnStyleLineUpdate(TextStyle dialogueStyle)
        {
            LineStyleUpdated?.Invoke(dialogueStyle);
        }
        #endregion
    }

    [Serializable]
    public class StyleUnityEvent : UnityEvent<TextStyle> { }
    [Serializable]
    public class StringUnityEvent : UnityEvent<string> { }
}