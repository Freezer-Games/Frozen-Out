namespace Assets.Scripts.Dialogue.Texts.Tags
{
    public class TagOption
    {
        public const char EQUAL_SIGN = '=';

        public string Option { get; set; }

        public string MainOption => 
            (Option == null) 
            ? null 
            : (Option.IndexOf(EQUAL_SIGN) < 0) 
                ? Option 
                : Option.Split(EQUAL_SIGN)[0];

        public TagOptionPosition Position { get; set; }

        public string Text
        {
            get
            {
                // No se puede convertir en expresión switch, porque Unity no lo entiende
                // (es una adición a CSharp muy nueva)
                switch (Position)
                {
                    case TagOptionPosition.start: return $"{Tag.SEPARATOR_INIT}{Option}{Tag.SEPARATOR_END}";
                    case TagOptionPosition.end: return $"{Tag.SEPARATOR_INIT}{Tag.OPTION_END}{Option}{Tag.SEPARATOR_END}";
                    default: return null;
                };
            }
        }

        public TagOption(string option, TagOptionPosition position = TagOptionPosition.start)
        {
            this.Option = option;
            this.Position = position;
        }

        public override string ToString() => this.Text;

        public static bool Matches(TagOption start, TagOption end)
        {
            return start.Position == TagOptionPosition.start
                && end.Position == TagOptionPosition.end
                && start.MainOption == end.MainOption;
        }

        public static TagOption ExtractTag(string line, int startIndex, out string remainingText)
        {
            string remainingTextWithStart = line.Substring(startIndex);

            int indexOfSeparatorStartEnd = Tag.IndexOfNextTagEnd(remainingTextWithStart);
            if (indexOfSeparatorStartEnd < 0) throw new ParsingException.StartTagSeparatorWithoutEndException(startIndex);

            string tagOptionFull = remainingTextWithStart.Substring(0, indexOfSeparatorStartEnd + 1);
            string tagOption = ExtractTagOption(tagOptionFull);

            TagOptionPosition tagOptionPosition;
            int indexOfOptionEnd = Tag.IndexOfNextOptionEnd(tagOptionFull);
            if (indexOfOptionEnd < 0)
            {
                tagOptionPosition = TagOptionPosition.start;
            }
            else
            {
                tagOptionPosition = TagOptionPosition.end;
                tagOption = tagOption.Remove(indexOfOptionEnd - 1, 1);
            }

            remainingText = remainingTextWithStart.Substring(indexOfSeparatorStartEnd + 1);

            return new TagOption(tagOption, tagOptionPosition);
        }

        private static string ExtractTagOption(string tagOptionFull) => tagOptionFull.Substring(1, tagOptionFull.Length - 2);
    }
}
