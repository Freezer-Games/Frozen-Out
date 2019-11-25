using Assets.Scripts.Dialogue.Texts;
using System;
using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public class DialogueSnippetSystem : MonoBehaviour
    {
        public TextAsset Snippets;

        public string ParseSnippets(string text, Action<ParsingException> logger = null)
        {
            string result = text, textBeingAnalyzed = text;
            int currentIndex = 0;

            while (textBeingAnalyzed.Length > 0)
            {
                // If something went wrong with the snippet, it would skip it
                int nextIndex = currentIndex + 1, indexOfSnippetInit = 0;
                try
                {
                    indexOfSnippetInit = textBeingAnalyzed.IndexOf(Snippet.Separator);
                    if (indexOfSnippetInit >= 0)
                    {
                        nextIndex = indexOfSnippetInit + 1;
                        string textSearchingForEnd = textBeingAnalyzed.Substring(nextIndex);
                        int indexOfSnippetEnd = textSearchingForEnd.IndexOf(Snippet.Separator);

                        if (indexOfSnippetEnd >= 0)
                        {
                            nextIndex = (textBeingAnalyzed.Length - textSearchingForEnd.Length) + indexOfSnippetEnd + 1;
                            string snippetName = textBeingAnalyzed.Substring(indexOfSnippetInit + 1, (nextIndex - 2) - indexOfSnippetInit);

                            Snippet snippet = new Snippet(snippetName);
                            snippet.LoadValueFrom(Snippets.text);

                            result = snippet.Replace(result);
                        }
                        else
                        {
                            throw new ParsingException(((textBeingAnalyzed.Length - textSearchingForEnd.Length) + indexOfSnippetInit), "Snippet init without end");
                        }
                    }
                    else
                    {
                        nextIndex = -1;
                    }
                }
                catch (ParsingException ex)
                {
                    // Log the exception
                    logger?.Invoke(ex);
                    Console.WriteLine(ex.Message);

                    nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfSnippetInit) + 1;
                }

                // Go to the next portion of the text (Skip the exception source)
                textBeingAnalyzed = nextIndex >= 0 && nextIndex < textBeingAnalyzed.Length ? textBeingAnalyzed.Substring(nextIndex) : "";
                currentIndex = nextIndex;
            }

            return result;
        }
    }
}
