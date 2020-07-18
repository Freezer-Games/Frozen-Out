using System;
using System.Collections.Generic;

using Scripts.Level.Dialogue.Utils.Tag;

namespace Scripts.Level.Dialogue.Utils
{
    public class DialogueTaggedText : IDialogueText
    {
        public DialogueTaggedText(TagType tag, string text) : this(tag, new DialogueText(text))
        {

        }

        public DialogueTaggedText(TagType tag, IDialogueText dialogueText = null)
        {
            this.Tag = tag;
            this.Text = dialogueText;
        }

        public IDialogueText Text
        {
            get;
            private set;
        }
        public TagType Tag
        {
            get;
            private set;
        }

        public void AddText(string text)
        {
            if (this.Text == null)
            {
                this.Text = new DialogueText(text);
            }   
            else
            {
                this.Text.AddText(text);
            }   
        }

        public void AddText(IDialogueText dialogueText)
        {
            if (this.Text == null)
            {
                this.Text = dialogueText;
            }   
            else
            {
                this.Text.AddText(dialogueText);
            }
        }

        public IEnumerable<string> ParseAccumulated() => Tag.Parse(Text.ParseAccumulated); // Parsear acorde con el tag

        public IEnumerable<string> ParseSingle() => Tag.Parse(Text.ParseSingle);

        public string ToStringClean() => Text.ToStringClean();
        public string ToStringFull() => Tag?.GetTaggedText(Text.ToStringFull());
    }
}