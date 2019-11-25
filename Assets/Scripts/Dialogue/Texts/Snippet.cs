using System.IO;

namespace Assets.Scripts.Dialogue.Texts
{
    public class Snippet
    {
        public const char Separator = '%';
        private const string EqualSign = "===";

        public string Name { get; set; }

        public string Value { get; private set; }

        public string FullName => $"{Separator}{Name}{Separator}";

        public Snippet(string name)
        {
            Name = name;
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
                        Value = line.Substring(line.IndexOf(EqualSign) + EqualSign.Length);
                        return;
                    }
                }
            }
        }

        public string Replace(string text) => text.Replace(FullName, Value);
    }
}
