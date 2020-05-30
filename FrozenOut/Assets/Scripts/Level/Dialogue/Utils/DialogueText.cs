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
        public DialogueText(string text)
        {
            this.Text = text;
        }

        public string Text
        {
            get;
            private set;
        }

        public void AddText(string text)
        {
            this.Text += text;
        }

        public void AddText(IDialogueText dialogueText)
        {
            this.Text += dialogueText.ToStringFull();
        }

        /// <summary>
        /// Devuelve el texto letra a letra de forma acumulada
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseAccumulated()
        {
            string currentText = "";
            foreach (char letter in Text)
            {
                currentText += letter;
                yield return currentText;
            }
        }

        /// <summary>
        /// Devuelve el texto letra a letra sin acumular
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseSingle()
        {
            foreach (char letter in Text)
            {
                yield return letter.ToString();
            }
        }

        public string ToStringClean() => this.Text;
        public string ToStringFull() => this.Text;
    }
}