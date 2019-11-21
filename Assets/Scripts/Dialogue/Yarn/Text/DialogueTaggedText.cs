using Assets.Scripts.Dialogue.Texts.Tags;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Dialogue.Texts
{
    public class DialogueTaggedText : IDialogueText
    {
        public IDialogueText Text { get; set; }
        public Tag Tag { get; set; }

        public string FullText => Tag?.GetTaggedText(Text.ToString());

        public DialogueTaggedText(Tag tag, string text) : this(tag, new DialogueText(text))
        {
        }

        public DialogueTaggedText(Tag tag, IDialogueText dialogueText = null)
        {
            this.Tag = tag;
            this.Text = dialogueText;
        }

        public void AddText(string text)
        {
            if (this.Text == null)
                this.Text = new DialogueText(text);
            else
                this.Text.AddText(text);
        }

        public void AddDialogueText(IDialogueText dialogueText)
        {
            if (this.Text == null)
                this.Text = dialogueText;
            else
                this.Text.AddDialogueText(dialogueText);
        }

        public IEnumerable<string> ParseInBuilder(StringBuilder builder)
        {
            string endTag = Tag.EndOption.Text;
            builder.Append(Tag.StartOption.Text + endTag);
            foreach (char letter in Parse())
            {
                builder.Insert(builder.Length - endTag.Length, letter);
                yield return builder.ToString();
            }
        }

        public IEnumerable<char> Parse()
        {
            foreach (char letter in Text.Parse())
            {
                yield return letter;
            }
        }

        public override string ToString() => this.FullText;
    }
}
