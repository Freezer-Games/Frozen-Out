using System;
using System.Collections.Generic;

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

            format.Snippets.TryGetValue(Name, out T value);

            // Es posible que el map venga con el nombre completo, así que comprobamos por si acaso
            if (value == null)
                format.Snippets.TryGetValue(FullName, out value);

            if (value == null) 
                throw new KeyNotFoundException($"El Snippet de nombre \"{name}\" no tiene valor asociado.");
            else 
                Value = value;
        }
    }
}
