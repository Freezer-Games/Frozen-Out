using UnityEngine;

namespace Scripts.Level.Dialogue.Text.Unity
{
    [RequireComponent(typeof(Canvas))]
    public class BoxTextManager : TextManager
    {
        public Canvas DialogueCanvas;

        public UnityEngine.UI.Text NameText;
        public DialogueBoxController DialogueBoxController;

        private TextStyle CurrentStyle;

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
            DialogueBoxController.Clear(); //May want to mantain name
        }

        public override void SetStyle(TextStyle style)
        {
            CurrentStyle = style;
        }

        public override void ShowName(string name)
        {
            NameText.text = name;
        }

        public override void ShowDialogueAccumulated(string dialogue)
        {
            DialogueBoxController.Clear();
            foreach (char letter in dialogue)
            {
                ShowDialogueSingle(letter.ToString());
            }
        }

        public override void ShowDialogueSingle(string dialogueLetter)
        {
            DialogueBoxController.SetText(dialogueLetter, CurrentStyle);
        }

        private void Clear()
        {
            ClearName();
            DialogueBoxController.Clear();
        }

        private void ClearName()
        {
            NameText.text = "";
        }
    }
}