using System.IO;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    public class FileSnippetFormat : SnippetFormat<string>
    {
        public string NameValueSeparator { get; set; }

        public FileSnippetFormat(string startSeparator, string endSeparator, string nameValueSeparator) : base(startSeparator, endSeparator)
        {
            NameValueSeparator = nameValueSeparator;
        }

        public int IndexOfNextNameValueSeparator(string text) => text.IndexOf(NameValueSeparator);

        public void LoadSnippets(string text)
        {
            using (StringReader textReader = new StringReader(text))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    int splitIndex = IndexOfNextNameValueSeparator(line);
                    string name = line.Substring(0, splitIndex), value = line.Substring(splitIndex + NameValueSeparator.Length);
                    Snippets[name] = value;
                }
            }
        }
    }
}
