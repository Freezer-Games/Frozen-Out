using System;

namespace Assets.Scripts.Dialogue.Texts.Snippets
{
    [Serializable]
    public class Snippet<T>
    {
        public string Name { get; set; }

        public T Value { get; private set; }

        public string FullName => $"{Format.StartSeparator}{Name}{Format.EndSeparator}".Trim();

        public SnippetFormat<T> Format { get; set; }

        public Snippet(string name, SnippetFormat<T> format)
        {
            Name = name;
            Format = format;
            Value = format.Snippets[name];
        }
    }
}
