using System;

namespace Assets.Scripts.Dialogue.Texts.Tags
{
    public abstract class TagException : FormatException
    {
        public TagOption Tag { get; set; }

        public int Index { get; set; }

        public TagException(string message = null) : base(message)
        {

        }

        public abstract string GetFullMessage(int currentLineNumber);
    }
    public class EndTagBeforeStartException : TagException
    {
        public const string DEFAULT_MESSAGE = "Warning: End tag before start. Skipping tag.";

        public EndTagBeforeStartException(TagOption tag, int index = 0) : base(DEFAULT_MESSAGE)
        {
            this.Tag = tag;
            this.Index = index;
        }

        public override string GetFullMessage(int currentLineNumber)
            => $"Warning: End tag before start (line {currentLineNumber}, position {Index}). Skipping tag.";
    }

    public class StartTagWithoutEndException : TagException
    {
        public const string DEFAULT_MESSAGE = "Warning: Start tag without end. Skipping tag.";

        public StartTagWithoutEndException(TagOption tag, int index = 0) : base(DEFAULT_MESSAGE)
        {
            this.Tag = tag;
            this.Index = index;
        }

        public override string GetFullMessage(int currentLineNumber)
            => $"Warning: Start tag without end (line {currentLineNumber}, position {Index}). Skipping tag.";
    }
}
