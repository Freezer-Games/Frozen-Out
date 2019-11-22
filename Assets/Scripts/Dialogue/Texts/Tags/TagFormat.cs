namespace Assets.Scripts.Dialogue.Texts.Tags
{
    public class TagFormat
    {
        public string StartSeparator { get; }
        public string EndSeparator { get; }
        public string EndOptionSeparator { get; }

        public TagParsingStrategy Strategy { get; }

        public TagFormat(string startSeparator, string endSeparator, string endOptionSeparator, TagParsingStrategy strategy = TagParsingStrategy.Full)
        {
            StartSeparator = startSeparator;
            EndSeparator = endSeparator;
            EndOptionSeparator = endOptionSeparator;
            Strategy = strategy;
        }

        public int IndexOfNextStart(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextOptionEnd(string text) => text.IndexOf(EndOptionSeparator);
        public int IndexOfNextTagEnd(string text) => text.IndexOf(EndSeparator);

        public bool HasAnyTags(string text) => IndexOfNextStart(text) >= 0;


        public TagOption ExtractTag(string line, out int indexOfTagStart, out string remainingText)
        {
            indexOfTagStart = IndexOfNextStart(line);
            if (indexOfTagStart < 0)
            {
                remainingText = null;
                return null;
            }

            string remainingTextWithStart = line.Substring(indexOfTagStart + StartSeparator.Length);

            int indexOfSeparatorStartEnd = IndexOfNextTagEnd(remainingTextWithStart);
            if (indexOfSeparatorStartEnd < 0) throw new ParsingException.StartTagSeparatorWithoutEndException(indexOfTagStart);

            string tagOptionFull = line.Substring(indexOfTagStart, StartSeparator.Length + indexOfSeparatorStartEnd + EndSeparator.Length);
            string tagOption = ExtractTagOption(tagOptionFull);

            TagOptionPosition tagOptionPosition;
            int indexOfOptionEnd = IndexOfNextOptionEnd(tagOptionFull);
            if (indexOfOptionEnd < 0)
            {
                tagOptionPosition = TagOptionPosition.start;
            }
            else
            {
                tagOptionPosition = TagOptionPosition.end;
                tagOption = tagOption.Remove(indexOfOptionEnd - 1, EndOptionSeparator.Length);
            }

            remainingText = remainingTextWithStart.Substring(indexOfSeparatorStartEnd + EndSeparator.Length);

            return new TagOption(tagOption, this, tagOptionPosition);
        }

        private string ExtractTagOption(string tagOptionFull) => tagOptionFull.Substring(StartSeparator.Length, tagOptionFull.Length - 2);
    }
}
