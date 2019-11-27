using System;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Tags
{
    public class TagFormat : ISeparatedFormat<TagOption>
    {
        private const char RichTextTagSeparatorInit = '<';
        private const char RichTextTagSeparatorEnd = '>';
        private const char RichTextTagOptionEnd = '/';

        public static readonly TagFormat RichTextTagFormat = new TagFormat(
            RichTextTagSeparatorInit.ToString(),
            RichTextTagSeparatorEnd.ToString(),
            RichTextTagOptionEnd.ToString());

        public string StartSeparator { get; }
        public string EndSeparator { get; }
        public string EndOptionSeparator { get; }

        public ParsingStrategy Strategy { get; }

        public Func<string, string> Formatter { get; set; }

        public TagFormat(string startSeparator, string endSeparator, string endOptionSeparator, ParsingStrategy strategy = ParsingStrategy.Full, Func<string, string> formatter = null)
        {
            StartSeparator = startSeparator;
            EndSeparator = endSeparator;
            EndOptionSeparator = endOptionSeparator;
            Strategy = strategy;
            Formatter = formatter;
        }

        public int IndexOfNextStart(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextOptionEnd(string text) => text.IndexOf(EndOptionSeparator);
        public int IndexOfNextEnd(string text) => text.IndexOf(EndSeparator);

        public bool HasAnyTags(string text) => IndexOfNextStart(text) >= 0;


        public TagOption Extract(string line, out int indexOfTagStart, out int endIndex, out string remainingText)
        {
            indexOfTagStart = IndexOfNextStart(line);
            if (indexOfTagStart < 0)
            {
                remainingText = null;
                endIndex = -1;
                return null;
            }

            string remainingTextWithStart = line.Substring(indexOfTagStart + StartSeparator.Length);

            int indexOfSeparatorStartEnd = IndexOfNextEnd(remainingTextWithStart);
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

            endIndex = indexOfSeparatorStartEnd + EndSeparator.Length;
            remainingText = remainingTextWithStart.Substring(endIndex);

            return new TagOption(tagOption, this, tagOptionPosition);
        }

        private string ExtractTagOption(string tagOptionFull) => tagOptionFull.Substring(StartSeparator.Length, tagOptionFull.Length - 2);
    }
}
