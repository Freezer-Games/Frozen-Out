using Assets.Scripts.Dialogue.Texts.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Dialogue.Texts
{
    public class ComplexDialogueText : IDialogueText
    {
        public List<IDialogueText> Texts { get; set; }

        private string FullText => Texts.Aggregate("", (fullText, text) => fullText + text.ToString());

        public ComplexDialogueText()
        {

        }

        public ComplexDialogueText(List<IDialogueText> texts)
        {
            Texts = texts;
        }

        public ComplexDialogueText(params IDialogueText[] texts)
        {
            Texts = new List<IDialogueText>(texts);
        }

        /// <summary>
        /// Si los <paramref name="texts"/> no tienen ningún tag, se pueden pasar directamente como <see cref="string"/>, y este constructor los convertirá a <see cref="DialogueText"/> por ti.
        /// </summary>
        /// <param name="texts"></param>
        public ComplexDialogueText(params string[] texts)
        {
            Texts = new List<IDialogueText>(texts.Select(text => new DialogueText(text)));
        }

        public void AddText(string text)
        {
            this.AddDialogueText(new DialogueText(text));
        }

        public void AddDialogueText(IDialogueText dialogueText)
        {
            this.Texts.Add(dialogueText);
        }

        /// <summary>
        /// Obtiene el texto de todos los <see cref="IDialogueText"/>, y lo devuelve letra a letra.
        /// <para>Si el texto tiene tags (<see cref="DialogueTaggedText"/>),
        /// envolverá el texto en el tag para que el usuario nunca vea los caracteres asociados al mismo (los cuales no forman parte del texto).</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Parse()
        {
            string currentTotalText = "";
            foreach (IDialogueText text in Texts)
            {
                string currentText = "";
                foreach (string nextText in text.Parse())
                {
                    currentText = nextText;
                    yield return currentTotalText + nextText;
                }
                currentTotalText += currentText;
            }
        }

        public override string ToString() => FullText;

        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según el tipo de <see cref="IDialogueText"/> que es (si contiene o no tags, etc.)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDialogueText AnalyzeText(string text, Action<ParsingException> logger = null)
        {
            IDialogueText resultDialogueText = null;
            string textBeingAnalyzed = text;
            int currentIndex = 0;

            while (textBeingAnalyzed.Length > 0)
            {            
                int indexOfTagInit = textBeingAnalyzed.IndexOf(Tag.SEPARATOR_INIT);
                if (indexOfTagInit >= 0)
                {
                    int nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfTagInit) + 1;
                    try
                    {
                        TagOption tag = TagOption.ExtractTag(textBeingAnalyzed, indexOfTagInit, out string remainingTextAfterStart);

                        // If something went wrong with the tag, it would skip it
                        nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfTagInit) + tag.Text.Length;
                        if (indexOfTagInit > 0)
                        {
                            string textBeforeTag = textBeingAnalyzed.Substring(0, indexOfTagInit);
                            if (resultDialogueText == null)
                            {
                                resultDialogueText = new ComplexDialogueText(textBeforeTag);
                            }
                            else
                            {
                                resultDialogueText.AddText(textBeforeTag);
                            }
                        }

                        if (tag.Position == TagOptionPosition.start)
                        {
                            string textSearchingForEnd = remainingTextAfterStart;
                            string taggedText = null, remainingTextAfterEnd = null;
                            TagOption endTag = null;
                            while (taggedText == null && textSearchingForEnd.Length > 0)
                            {
                                int indexOfEndTagInit = textSearchingForEnd.IndexOf(Tag.SEPARATOR_INIT);
                                if (indexOfEndTagInit >= 0)
                                {
                                    endTag = TagOption.ExtractTag(textSearchingForEnd, indexOfEndTagInit, out remainingTextAfterEnd);
                                    if (TagOption.Matches(tag, endTag))
                                    {
                                        taggedText = remainingTextAfterStart.Substring(0, remainingTextAfterStart.Length - remainingTextAfterEnd.Length - endTag.Text.Length);
                                        textBeingAnalyzed = remainingTextAfterEnd; // This tag has been found correctly, go to the next portion of the text
                                    }
                                    else
                                    {
                                        textSearchingForEnd = remainingTextAfterEnd;
                                    }
                                }
                            }

                            if (taggedText == null)
                            {
                                throw new TagException.StartTagWithoutEndException(tag, indexOfTagInit);
                            }
                            else
                            {
                                DialogueTaggedText dialogueTaggedText = new DialogueTaggedText(new Tag(tag, endTag), AnalyzeText(taggedText, logger));
                                if (resultDialogueText == null)
                                {
                                    if (remainingTextAfterEnd != null && remainingTextAfterEnd.Length > 0)
                                    {
                                        resultDialogueText = new ComplexDialogueText(dialogueTaggedText);
                                    }
                                    else
                                    {
                                        resultDialogueText = dialogueTaggedText;
                                    }
                                }
                                else
                                {
                                    resultDialogueText.AddDialogueText(dialogueTaggedText);
                                }
                            }
                        }
                        else
                        {
                            throw new TagException.EndTagBeforeStartException(tag, currentIndex);
                        }
                    }
                    catch (ParsingException ex)
                    {
                        // Log the exception
                        logger?.Invoke(ex);
                        Console.WriteLine(ex.Message);

                        // Go to the next portion of the text (Skip the exception source)
                        textBeingAnalyzed = textBeingAnalyzed.Substring(nextIndex);
                        currentIndex = nextIndex;
                    }
                }
                else
                {
                    if (resultDialogueText == null)
                    {
                        return new DialogueText(textBeingAnalyzed);
                    }
                    else
                    {
                        resultDialogueText.AddText(textBeingAnalyzed);
                        return resultDialogueText;
                    }
                }
            }

            return resultDialogueText;
        }
    }
}
