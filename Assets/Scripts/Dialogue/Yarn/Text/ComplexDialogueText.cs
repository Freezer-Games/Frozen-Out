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
            this.Texts.Add(new DialogueText(text));
        }

        public void AddDialogueText(IDialogueText dialogueText)
        {
            this.Texts.Add(dialogueText);
        }

        /// <summary>
        /// Obtiene el texto de todos los <see cref="DialogueText"/>, y lo va actualizando letra a letra en el <paramref name="builder"/>.
        /// <para>En este método, si el texto tiene tags (<see cref="DialogueTaggedText"/>),
        /// envolverá el texto en el tag para que el usuario nunca vea los caracteres asociados al mismo (los cuales no forman parte del texto).</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseInBuilder(StringBuilder builder)
        {
            foreach (DialogueText text in Texts)
            {
                foreach (string currentText in text.ParseInBuilder(builder))
                {
                    yield return currentText;
                }
            }
        }

        /// <summary>
        /// Obtiene el texto de todos los <see cref="IDialogueText"/>, y lo devuelve letra a letra.
        /// <para>En este método, no importa si el texto tiene tags (<see cref="DialogueTaggedText"/>), devolverá cada carácter uno por uno.</para>
        /// <para>Para un comportamiento más útil para mostrar en elementos de la UI, es mejor utilizar <see cref="ParseInBuilder(StringBuilder)"/>.</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<char> Parse()
        {
            foreach (IDialogueText text in Texts)
            {
                foreach (char letter in text.Parse())
                {
                    yield return letter;
                }
            }
        }

        public override string ToString() => FullText;

        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según el tipo de <see cref="IDialogueText"/> que es (si contiene o no tags, etc.)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDialogueText AnalyzeText(string text, Action<TagException> logger = null)
        {
            IDialogueText resultDialogueText = null;
            string textBeingAnalyzed = text;
            int currentIndex = 0;

            while (textBeingAnalyzed.Length > 0)
            {            
                int indexOfTagInit = textBeingAnalyzed.IndexOf(Tag.SEPARATOR_INIT);
                if (indexOfTagInit >= 0)
                {
                    TagOption tag = TagOption.ExtractTag(textBeingAnalyzed, indexOfTagInit, out string remainingTextAfterStart);

                    // If something went wrong with the tag, it would skip it
                    int nextIndex = indexOfTagInit + tag.Text.Length;
                    try
                    {
                        if (indexOfTagInit > 0)
                        {
                            string textBeforeTag = text.Substring(0, indexOfTagInit);
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
                            string taggedText = null;
                            while (taggedText == null && textSearchingForEnd.Length > 0)
                            {
                                int indexOfEndTagInit = textSearchingForEnd.IndexOf(Tag.SEPARATOR_INIT);
                                if (indexOfEndTagInit >= 0)
                                {
                                    TagOption endTag = TagOption.ExtractTag(textSearchingForEnd, indexOfEndTagInit, out string remainingTextAfterEnd);
                                    if (endTag.Position == TagOptionPosition.end && tag.Option == endTag.Option)
                                    {
                                        taggedText = remainingTextAfterStart.Substring(0, remainingTextAfterStart.Length - remainingTextAfterEnd.Length - endTag.Text.Length);
                                        nextIndex = textBeingAnalyzed.Length - remainingTextAfterEnd.Length; // This tag has been found correctly, go to the next portion of the text
                                    }
                                    else
                                    {
                                        textSearchingForEnd = remainingTextAfterEnd;
                                    }
                                }
                            }

                            if (taggedText == null)
                            {
                                throw new StartTagWithoutEndException(tag, indexOfTagInit);
                            }
                            else
                            {
                                DialogueTaggedText dialogueTaggedText = new DialogueTaggedText(new Tag(tag.Option), AnalyzeText(taggedText, logger));
                                if (resultDialogueText == null) resultDialogueText = dialogueTaggedText;
                                else resultDialogueText.AddDialogueText(dialogueTaggedText);
                            }
                        }
                        else
                        {
                            throw new EndTagBeforeStartException(tag, currentIndex);
                        }
                    }
                    catch (TagException ex)
                    {
                        // Log the warning
                        logger?.Invoke(ex);
                        Console.WriteLine(ex.Message);
                    }

                    // Go to the next portion of the text (depending on what happened)
                    textBeingAnalyzed = textBeingAnalyzed.Substring(nextIndex);
                    currentIndex = nextIndex;
                }
                else
                {
                    return new DialogueText(textBeingAnalyzed);
                }
            }

            return resultDialogueText;
        }
    }
}
