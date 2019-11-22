namespace Assets.Scripts.Dialogue.Texts.Tags
{
    public class TagFormat
    {
        public char StartSeparator { get; }
        public char EndSeparator { get; }
        public char EndOptionSeparator { get; }

        public TagFormat(char startSeparator, char endSeparator, char endOptionSeparator)
        {
            StartSeparator = startSeparator;
            EndSeparator = endSeparator;
            EndOptionSeparator = endOptionSeparator;
        }

        public int IndexOfNextTagInit(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextOptionEnd(string text) => text.IndexOf(EndOptionSeparator);
        public int IndexOfNextTagEnd(string text) => text.IndexOf(EndSeparator);

        public bool HasAnyTags(string text) => IndexOfNextTagInit(text) >= 0;


        public TagOption ExtractTag(string line, int startIndex, out string remainingText)
        {
            string remainingTextWithStart = line.Substring(startIndex);

            int indexOfSeparatorStartEnd = IndexOfNextTagEnd(remainingTextWithStart);
            if (indexOfSeparatorStartEnd < 0) throw new ParsingException.StartTagSeparatorWithoutEndException(startIndex);

            string tagOptionFull = remainingTextWithStart.Substring(0, indexOfSeparatorStartEnd + 1);
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
                tagOption = tagOption.Remove(indexOfOptionEnd - 1, 1);
            }

            remainingText = remainingTextWithStart.Substring(indexOfSeparatorStartEnd + 1);

            return new TagOption(tagOption, this, tagOptionPosition);
        }


        private static string ExtractTagOption(string tagOptionFull) => tagOptionFull.Substring(1, tagOptionFull.Length - 2);
    }
}
