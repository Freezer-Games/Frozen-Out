using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Dialogue.Text.Unity
{
    [RequireComponent(typeof(Canvas))]
    public class BoxTextManager : TextManager
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
            OnDialogueEnd();
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
            OnDialogueLineUpdate(dialogue);
        }

        public override void ShowDialogueSingle(string dialogueLetter)
        {
            OnDialogueLetterUpdate(dialogueLetter);
        }

        #region Events
        public UnityEvent DialogueStarted;
        public UnityEvent DialogueEnded;
        public StringUnityEvent LineNameUpdated;
        public StringUnityEvent LineDialogueUpdated;
        public StringUnityEvent LetterDialogueUpdated;
        public StyleUnityEvent LineStyleUpdated;

        private void OnDialogueStart()
        {
            DialogueStarted?.Invoke();
        }

        private void OnDialogueEnd()
        {
            DialogueEnded?.Invoke();
        }

        private void OnNameLineUpdate(string nameToDisplay)
        {
            LineNameUpdated?.Invoke(nameToDisplay);
        }

        private void OnDialogueLineUpdate(string dialogueToDisplay)
        {
            LineDialogueUpdated?.Invoke(dialogueToDisplay);
        }

        private void OnDialogueLetterUpdate(string letterToDisplay)
        {
            LetterDialogueUpdated?.Invoke(letterToDisplay);
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