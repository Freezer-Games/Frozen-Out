using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Level.Dialogue.Utils
{
    [Serializable]
    public class ParsingException : FormatException
    {
        public const string END_ACTION_MESSAGE = "Skipping source.";

        public int? Index { get; set; }

        public ParsingException(int? index = null, string message = null) : base(message)
        {
            Index = index;
        }

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