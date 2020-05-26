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
            OnDialogueStart();
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

        public override void ShowDialogueAccumulated(string dialogue)
        {
            // TODO with textstyle
            OnDialogueLineUpdate(dialogue);
        }

        public override void ShowDialogueSingle(string newDialogueLetter)
        {
            OnDialogueLineUpdate(newDialogueLetter);
            // TODO
            // Create animator for text
            // Create prefab of Text with animator
            // Instantiate different Text gameobject for each letter
        }

        #region Events
        public UnityEvent DialogueStarted;
        public StringUnityEvent LineNameUpdated;
        public StringUnityEvent LineDialogueUpdated;
        public StyleUnityEvent LineStyleUpdated;

        private void OnDialogueStart()
        {
            DialogueStarted?.Invoke();
        }

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