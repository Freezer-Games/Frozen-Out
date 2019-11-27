using System.Collections.Generic;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class SnippetFormat<T> : ISeparatedFormat<Snippet<T>>
    {
        public static readonly SnippetFormat<object> VariableYarnFormat = new SnippetFormat<object>("$", " ");

        public string StartSeparator { get; }
        public string EndSeparator { get; }

        public Dictionary<string, T> Snippets { get; set; }

        public SnippetFormat(string startSeparator, string endSeparator)
        {
            StartSeparator = startSeparator;
            EndSeparator = endSeparator;

            Snippets = new Dictionary<string, T>();
        }

        public int IndexOfNextStart(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextEnd(string text) => text.IndexOf(EndSeparator);

        public bool HasAnyTags(string text) => IndexOfNextStart(text) >= 0;

        public Snippet<T> Extract(string line, out int startingIndex, out int endIndex, out string remainingText)
        {
            Snippet<T> snippet = null;

            startingIndex = IndexOfNextStart(line);
            if (startingIndex >= 0)
            {
                int nextIndex = startingIndex + 1;
                string textSearchingForEnd = line.Substring(nextIndex);
                int indexOfSnippetEnd = IndexOfNextEnd(textSearchingForEnd);

                if (indexOfSnippetEnd >= 0)
                {
                    nextIndex = line.Length - textSearchingForEnd.Length + indexOfSnippetEnd + 1;
                    string snippetName = line.Substring(startingIndex + 1, nextIndex - 2 - startingIndex);

                    endIndex = indexOfSnippetEnd + EndSeparator.Length;
                    remainingText = textSearchingForEnd.Substring(endIndex);
                    snippet = new Snippet<T>(snippetName, this);
                }
                else
                {
                    throw new ParsingException(line.Length - textSearchingForEnd.Length + startingIndex, "Snippet init without end");
                }
            }
            else
            {
                remainingText = line;
                endIndex = -1;
            }

            return snippet;
        }
    }
}
