using System.IO;

namespace Assets.Scripts.Dialogue.Texts
{
    public class Snippet
    {
        public string Name { get; set; }

        public string Value { get; private set; }

        public string FullName => $"{Format.StartSeparator}{Name}{Format.EndSeparator}";

        public SnippetFormat Format { get; set; }

        public Snippet(string name, SnippetFormat format)
        {
            Name = name;
            Format = format;
        }

        public void LoadValueFrom(string text)
        {
            using (StringReader textReader = new StringReader(text))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    if (line.StartsWith(Name))
                    {
                        Value = line.Substring(Format.IndexOfNextNameValueSeparator(line) + Format.NameValueSeparator.Length);
                        return;
                    }
                }
            }
        }

        public string Replace(string text) => text.Replace(FullName, Value);
    }
}
