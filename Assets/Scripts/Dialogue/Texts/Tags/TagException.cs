using System;

namespace Assets.Scripts.Dialogue.Texts.Tags
{
    [Serializable]
    public class TagException : ParsingException
    {
        public new const string END_ACTION_MESSAGE = "Skipping tag.";

        public TagOption Tag { get; set; }

        public TagException(TagOption tag, int? index = null, string message = null) : base(index, message)
        {
            this.Tag = tag;
        }

        public override string GetFullMessage(int currentLineNumber)
            => GetFullMessage(currentLineNumber, END_ACTION_MESSAGE);

        [Serializable]
        public class EndTagBeforeStartException : TagException
        {
            public const string DEFAULT_MESSAGE = "End tag before start";

            public EndTagBeforeStartException(TagOption tag, int? index = null) : base(tag, index, DEFAULT_MESSAGE)
            {
            }
        }

        [Serializable]
        public class StartTagWithoutEndException : TagException
        {
            public const string DEFAULT_MESSAGE = "Start tag without end";
            public StartTagWithoutEndException(TagOption tag, int? index = null) : base(tag, index, DEFAULT_MESSAGE)
            {
            }
        }
    }
}
