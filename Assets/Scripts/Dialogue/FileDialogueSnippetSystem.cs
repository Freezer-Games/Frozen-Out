using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Texts.Snippets;
using System;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class FileDialogueSnippetSystem : DialogueSnippetSystem<string>
    {
        private const string DefaultNameValueSeparator = "===";

        public string NameValueSeparator = DefaultNameValueSeparator;

        public TextAsset Snippets;

        protected override void Start()
        {
            FileSnippetFormat fileFormat = new FileSnippetFormat(StartSeparator, EndSeparator, NameValueSeparator);
            fileFormat.LoadSnippets(Snippets.text);
            Format = fileFormat;
        }

        public new string ParseSnippets(string text, Action<ParsingException> logger = null)
        {
            string result = text;

            var snippets = base.ParseSnippets(text, logger);
            foreach (Snippet<string> snippet in snippets)
            {
                result = result.Replace(snippet.FullName, snippet.Value);
            }

            return result;
        }
    }
}
