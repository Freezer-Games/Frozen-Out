using Assets.Scripts.Dialogue.Texts;
using System;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class DialogueSnippetSystem : MonoBehaviour
    {
        public const string DefaultSeparator = "%";
        private const string DefaultNameValueSeparator = "===";

        public TextAsset Snippets;
        public SnippetFormat Format = new SnippetFormat(DefaultSeparator, DefaultSeparator, DefaultNameValueSeparator);

        public string ParseSnippets(string text, Action<ParsingException> logger = null)
        {
            string result = text, textBeingAnalyzed = text;
            int currentIndex = 0;

            while (textBeingAnalyzed != null && textBeingAnalyzed.Length > 0)
            {
                // If something went wrong with the snippet, it would skip it
                int nextIndex = currentIndex + 1, indexOfSnippetInit = 0;
                try
                {
                    Snippet snippet = Format.Extract(textBeingAnalyzed, out indexOfSnippetInit, out nextIndex, out string remainingText);
                    if (snippet != null)
                    {
                        snippet.LoadValueFrom(Snippets.text);
                        result = snippet.Replace(result);
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
