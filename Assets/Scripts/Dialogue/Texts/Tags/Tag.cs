using System;
using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Tags
{
    /// <summary>
    /// Tags disponibles en Unity: https://docs.unity3d.com/Manual/StyledText.html
    /// </summary>
    public class Tag
    {
        public string Option { get; }

        public TagFormat Format { get; set; }

        public TagOption StartOption { get; }
        public TagOption EndOption { get; }

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public Tag(string option, TagFormat format)
        {
            this.Option = option;
            this.Format = format;

            this.StartOption = new TagOption(option, format, TagOptionPosition.start);
            this.EndOption = new TagOption(option, format, TagOptionPosition.end);
        }

        public Tag(TagOption startOption, TagOption endOption)
        {
            this.Option = startOption.MainOption;
            this.Format = startOption.Format;

            this.StartOption = startOption;
            this.EndOption = endOption;
        }

        public string GetTaggedText(string text) => StartOption.Text + text + EndOption.Text;

        public IEnumerable<string> Parse(Func<IEnumerable<string>> textFeeder)
        {
            IEnumerable<string> FormattedTextFeeder()
            {
                foreach (string nextText in textFeeder())
                {
                    yield return Format.Formatter(nextText);
                }
            }

            if (Format?.Formatter != null) textFeeder = FormattedTextFeeder;

            switch (Format.Strategy)
            {
                case ParsingStrategy.Clean: return ParseClean(textFeeder);
                default: return ParseFull(textFeeder);
            }
        }

        private IEnumerable<string> ParseFull(Func<IEnumerable<string>> textFeeder)
        {
            foreach (string nextText in textFeeder())
            {
                yield return $"{StartOption.Text}{nextText}{EndOption.Text}";
            }
        }

        private IEnumerable<string> ParseClean(Func<IEnumerable<string>> textFeeder)
        {
            foreach (string nextText in textFeeder())
            {
                yield return nextText;
            }
        }
    }
}
