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

        public IEnumerable<string> Parse()
        {
            string startTag = Tag.StartOption.Text;
            string endTag = Tag.EndOption.Text;

            foreach (string nextText in Text.Parse())
            {
                yield return $"{startTag}{nextText}{endTag}";
            }
        }

        public override string ToString() => this.FullText;
    }
}
