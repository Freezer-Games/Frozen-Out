using Assets.Scripts.Dialogue.Texts;
using Assets.Scripts.Dialogue.Texts.Snippets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class DialogueSnippetSystem<T> : MonoBehaviour
    {
        public const string DefaultSeparator = "%";

        public string StartSeparator = DefaultSeparator;
        public string EndSeparator = DefaultSeparator;

        [Serializable]
        public struct SimpleSnippet
        {
            public string name;
            public string value;
        }

        protected readonly Dictionary<string, T> snippets = new Dictionary<string, T>();

        public SnippetFormat<T> Format { get; set; }

        protected virtual void Start()
        { 
            Format = new SnippetFormat<T>(StartSeparator, EndSeparator)
            {
                Snippets = snippets
            };
        }

        public List<Snippet<T>> ParseSnippets(string text, Action<ParsingException> logger = null)
        {
            List<Snippet<T>> result = new List<Snippet<T>>();
            string textBeingAnalyzed = text;
            int currentIndex = 0;

            while (textBeingAnalyzed != null && textBeingAnalyzed.Length > 0)
            {
                // If something went wrong with the snippet, it would skip it
                int nextIndex = currentIndex + 1, indexOfSnippetInit = 0;
                try
                {
                    Snippet<T> snippet = Format.Extract(textBeingAnalyzed, out indexOfSnippetInit, out nextIndex, out string remainingText);
                    if (snippet != null)
                    {
                        result.Add(snippet);
                    }
                    else
                    {
                        remainingText = null;
                    }

                    textBeingAnalyzed = remainingText;
                }
                catch (ParsingException ex)
                {
                    // Log the exception
                    logger?.Invoke(ex);
                    Console.WriteLine(ex.Message);

                    nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfSnippetInit) + 1;
                    textBeingAnalyzed = nextIndex >= 0 && nextIndex < textBeingAnalyzed.Length ? textBeingAnalyzed.Substring(nextIndex) : "";
                }

                // Go to the next portion of the text (Skip the exception source)
                currentIndex = nextIndex;
            }

            return result;
        }
    }
}
