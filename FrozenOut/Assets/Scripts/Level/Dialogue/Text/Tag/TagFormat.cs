using System;
using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Text.Tag
{
    /// <summary>
    /// Representa la forma en que los tags estan descritos (separadores y estrategia de parseo)
    /// </summary>
    public class TagFormat
    {
        private const char RichTextTagSeparatorInit = '<';
        private const char RichTextTagSeparatorEnd = '>';
        private const char RichTextTagOptionEnd = '/';

        public static readonly TagFormat RichTextTagFormat = new TagFormat(
            RichTextTagSeparatorInit.ToString(),
            RichTextTagSeparatorEnd.ToString(),
            RichTextTagOptionEnd.ToString()
        );

        public TagFormat(string startSeparator, string endSeparator, string endOptionSeparator, ParsingStrategy strategy = ParsingStrategy.Full, Func<string, string> formatter = null)
        {
            this.StartSeparator = startSeparator;
            this.EndSeparator = endSeparator;
            this.EndOptionSeparator = endOptionSeparator;
            this.Strategy = strategy;
            this.Formatter = formatter;
        }

        public string StartSeparator
        {
            get;
            private set;
        }
        public string EndSeparator
        {
            get;
            private set;
        }
        public string EndOptionSeparator
        {
            get;
            private set;
        }
        public ParsingStrategy Strategy
        {
            get;
            private set;
        }
        public Func<string, string> Formatter
        {
            get;
            private set;
        }

        public int IndexOfNextStart(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextOptionEnd(string text) => text.IndexOf(EndOptionSeparator);
        public int IndexOfNextEnd(string text) => text.IndexOf(EndSeparator);

        public bool HasAnyTags(string text) => IndexOfNextStart(text) >= 0;

        private string ExtractTagOption(string tagOptionFull) => tagOptionFull.Substring(StartSeparator.Length, tagOptionFull.Length - 2);

        /// Extraer la opción del tag (ej.: size, color)
        public TagOption Extract(string line, out int indexOfTagStart, out int endIndex, out string remainingText)
        {
            indexOfTagStart = IndexOfNextStart(line);
            // Si no hay separador inicial
            if (indexOfTagStart < 0)
            {
                remainingText = null;
                endIndex = -1;
                return null;
            }

            // ej.: "Esto es una <size=20>linea</size>"
            // remainigTextWithStart -> "size=20>linea</size>"
            string remainingTextWithStart = line.Substring(indexOfTagStart + StartSeparator.Length);

            int indexOfSeparatorStartEnd = IndexOfNextEnd(remainingTextWithStart);
            if (indexOfSeparatorStartEnd < 0) throw new ParsingException.StartTagSeparatorWithoutEndException(indexOfTagStart);

            // tagOptionFull -> "<size=20>l"
            string tagOptionFull = line.Substring(indexOfTagStart, indexOfSeparatorStartEnd + StartSeparator.Length + EndSeparator.Length);
            // tagOption -> "size=20"
            string tagOption = ExtractTagOption(tagOptionFull);

            TagOptionPosition tagOptionPosition;
            int indexOfOptionEnd = IndexOfNextOptionEnd(tagOptionFull);
            // Determinar que tipo de tag es
            // Si no se encuentra '/'
            if (indexOfOptionEnd < 0)
            {
                tagOptionPosition = TagOptionPosition.Start;
            }
            else
            {
                tagOptionPosition = TagOptionPosition.End;
                tagOption = tagOption.Remove(indexOfOptionEnd - 1, EndOptionSeparator.Length); // Quita el '/' de tagOption
            }

            endIndex = indexOfSeparatorStartEnd + EndSeparator.Length;
            // remainigText -> linea</size>
            remainingText = remainingTextWithStart.Substring(endIndex);

            // Devuelve el tag option interno
            return new TagOption(tagOption, this, tagOptionPosition);
        }
    }
}
