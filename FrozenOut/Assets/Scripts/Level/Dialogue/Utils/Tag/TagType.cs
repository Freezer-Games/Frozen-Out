using System;
using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Utils.Tag
{
    /// <summary>
    /// Representa un tipo de tag (ej. <></> o [][/]) y busca este tipo de tags dado un texto
    /// Tags disponibles en Unity: https://docs.unity3d.com/Manual/StyledText.html
    /// </summary>
    public class TagType
    {
        public TagType(string option, TagFormat format)
        {
            this.Option = option;
            this.Format = format;

            this.StartOption = new TagOption(option, format, TagOptionPosition.Start);
            this.EndOption = new TagOption(option, format, TagOptionPosition.End);
        }

        public TagType(TagOption startOption, TagOption endOption)
        {
            this.Option = startOption.MainOption();
            this.Format = startOption.Format;

            this.StartOption = startOption;
            this.EndOption = endOption;
        }

        public string Option
        {
            get;
            private set;
        }
        public TagFormat Format
        {
            get;
            private set;
        }
        public TagOption StartOption
        {
            get;
            private set;
        }
        public TagOption EndOption
        {
            get;
            private set;
        }

        public Dictionary<string, string> Attributes
        {
            get;
            private set;
        } = new Dictionary<string, string>();

        public string GetTaggedText(string text) => StartOption.Text() + text + EndOption.Text();

        public IEnumerable<string> Parse(Func<IEnumerable<string>> textFeeder)
        {
            // Declare an IEnumerable, NOT executing it
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
                case ParsingStrategy.Clean:
                    return ParseClean(textFeeder);
                case ParsingStrategy.Full:
                default:
                    return ParseFull(textFeeder);
            }
        }

        private IEnumerable<string> ParseFull(Func<IEnumerable<string>> textFeeder)
        {
            foreach (string nextText in textFeeder()) // foreach character given, it'll enclose it in the tag
            {
                yield return GetTaggedText(nextText);
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