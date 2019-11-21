using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dialogue.Texts
{
    [Serializable]
    public class ParsingException : FormatException
    {
        public const string END_ACTION_MESSAGE = "Skipping source.";

        public int? Index { get; set; }

        public ParsingException(int? index = null, string message = null) : base(message)
        {
            this.Index = index;
        }

        public virtual string GetFullMessage(int currentLineNumber)
            => GetFullMessage(currentLineNumber, END_ACTION_MESSAGE);

        protected string GetFullMessage(int currentLineNumber, string endActionMessage)
            => $"{Message} (line {currentLineNumber}, position {Index}). {endActionMessage}";

        [Serializable]
        public class StartTagSeparatorWithoutEndException : ParsingException
        {
            public const string DEFAULT_MESSAGE = "Start tag separator without end";
            public StartTagSeparatorWithoutEndException(int? index = null) : base(index, DEFAULT_MESSAGE)
            {
            }
        }
    }
}
