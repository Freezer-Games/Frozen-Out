using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Level.Dialogue.Utils
{
    /// <summary>
    /// Represents a simple Dialogue with a text
    /// </summary>
    public class DialogueText : IDialogueText
    {
        public string Text { get; set; }

        public DialogueText(string text)
        {
            this.Text = text;
        }

        public void AddText(string text)
        {
            this.Text += text;
        }

        public void AddText(IDialogueText dialogueText)
        {
            this.Text += dialogueText.ToString();
        }

        /// <summary>
        /// Devuelve el texto letra a letra
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Parse()
        {
            string currentText = "";
            foreach (char letter in Text)
            {
                currentText += letter;
                yield return currentText;
            }
        }

        public override string ToString() => this.Text;
    }
}