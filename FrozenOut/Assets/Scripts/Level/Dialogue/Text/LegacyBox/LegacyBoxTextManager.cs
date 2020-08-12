using UnityEngine;

namespace Scripts.Level.Dialogue.Text.Unity
{
    [RequireComponent(typeof(Canvas))]
    public class LegacyBoxTextManager : TextManager
    {
        public Canvas DialogueCanvas;

        public UnityEngine.UI.Text NameText;
        public UnityEngine.UI.Text DialogueText;

        void Awake()
        {
            Close();
        }

        public override void Open()
        {
            DialogueCanvas.enabled = true;
            Clear();
        }

        public override void Close()
        {
            DialogueCanvas.enabled = false;
        }

        public override void StartLine()
        {
            ClearDialogue(); //May want to mantain name
        }

        public override void SetStyle(TextStyle style)
        {
            DialogueText.font = style.Font;
            DialogueText.fontSize = style.Size;
            DialogueText.color = style.Colour;
        }

        public override void ShowName(string name)
        {
            NameText.text = name;
        }

        public override void ShowDialogueAccumulated(string dialogue)
        {
            ClearDialogue();
            foreach (char letter in dialogue)
            {
                ShowDialogueSingle(letter.ToString());
            }
        }

        public override void ShowDialogueSingle(string dialogueLetter)
        {
            DialogueText.text += dialogueLetter;
        }

        private void Clear()
        {
            ClearName();
            ClearDialogue();
        }

        private void ClearName()
        {
            NameText.text = "";
        }

        private void ClearDialogue()
        {
            DialogueText.text = "";
        }
    }
}