using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dialogue.Yarn
{
    public class TagOption
    {
        public string Option { get; set; }
        public TagOptionPosition Position { get; set; }

        public string Text => Position switch
        {
            TagOptionPosition.start => Tag.SEPARATOR_INIT + Option + Tag.SEPARATOR_END,
            TagOptionPosition.end => Tag.SEPARATOR_INIT + Tag.OPTION_END + Option + Tag.SEPARATOR_END,
            _ => null,
        };

        public TagOption(string option, TagOptionPosition position = TagOptionPosition.start)
        {
            this.Option = option;
            this.Position = position;
        }    

        public override string ToString() => base.ToString();

        public static TagOption ExtractTag(string line, int startIndex, out string remainingText)
        {
            string remainingTextWithStart = line.Substring(startIndex);

            int indexOfSeparatorStartEnd = remainingTextWithStart.IndexOf(Tag.SEPARATOR_END);
            string tagOptionFull = remainingTextWithStart.Substring(0, indexOfSeparatorStartEnd + 1);
            string tagOption = ExtractTagOption(tagOptionFull);

            TagOptionPosition tagOptionPosition;
            int indexOfOptionEnd = tagOptionFull.IndexOf(Tag.OPTION_END);
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
