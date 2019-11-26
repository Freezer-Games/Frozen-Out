namespace Assets.Scripts.Dialogue.Texts
{
    public class SnippetFormat : ISeparatedFormat<Snippet>
    {
        public string StartSeparator { get; }
        public string EndSeparator { get; }

        public string NameValueSeparator { get; }

        public SnippetFormat(string startSeparator, string endSeparator, string nameValueSeparator)
        {
            StartSeparator = startSeparator;
            EndSeparator = endSeparator;
            NameValueSeparator = nameValueSeparator;
        }

        public int IndexOfNextStart(string text) => text.IndexOf(StartSeparator);
        public int IndexOfNextEnd(string text) => text.IndexOf(EndSeparator);
        public int IndexOfNextNameValueSeparator(string text) => text.IndexOf(NameValueSeparator);

        public bool HasAnyTags(string text) => IndexOfNextStart(text) >= 0;


        public Snippet Extract(string line, out int startingIndex, out int endIndex, out string remainingText)
        {
            Snippet snippet = null;

            startingIndex = IndexOfNextStart(line);
            if (startingIndex >= 0)
            {
                int nextIndex = startingIndex + 1;
                string textSearchingForEnd = line.Substring(nextIndex);
                int indexOfSnippetEnd = IndexOfNextEnd(textSearchingForEnd);

                if (indexOfSnippetEnd >= 0)
                {
                    nextIndex = (line.Length - textSearchingForEnd.Length) + indexOfSnippetEnd + 1;
                    string snippetName = line.Substring(startingIndex + 1, (nextIndex - 2) - startingIndex);

                    endIndex = indexOfSnippetEnd + EndSeparator.Length;
                    remainingText = textSearchingForEnd.Substring(endIndex);
                    snippet = new Snippet(snippetName, this);
                }
                else
                {
                    throw new ParsingException(((line.Length - textSearchingForEnd.Length) + startingIndex), "Snippet init without end");
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
