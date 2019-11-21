using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dialogue.Texts
{
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

        public void AddDialogueText(IDialogueText dialogueText)
        {
            this.Text += dialogueText.ToString();
        }

        public IEnumerable<string> ParseInBuilder(StringBuilder builder)
        {
            foreach (char letter in Parse())
            {
                builder.Append(letter);
                yield return letter.ToString();
            }
        }

        public IEnumerable<char> Parse()
        {
            foreach (char letter in Text)
            {
                yield return letter;
            }
        }

        public override string ToString() => this.Text;
    }
}
