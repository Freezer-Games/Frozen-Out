using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Dialogue.Yarn
{
    public class DialogueTaggedText : DialogueText
    {
        public Tag Tag { get; set; }

        public string FullText => Tag?.GetTaggedText(Text);

        public DialogueTaggedText(Tag tag, string text = null) : base(text)
        {
            this.Tag = tag;
        }

        public override IEnumerable<string> ParseInBuilder(StringBuilder builder)
        {
            string endTag = Tag.EndOption.Text;
            builder.Append(Tag.StartOption.Text + endTag);
            foreach (char letter in Parse())
            {
                builder.Insert(builder.Length - endTag.Length, letter);
                yield return builder.ToString();
            }
        }

        public override string ToString() => this.FullText;
    }
}
