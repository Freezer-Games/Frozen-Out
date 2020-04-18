using System;
using System.Collections.Generic;

using Scripts.Level.Dialogue.Text.Tag;

namespace Scripts.Level.Dialogue.Text
{
    public class DialogueTaggedText : IDialogueText
    {
        public DialogueTaggedText(TagType tag, string text) : this(tag, new DialogueText(text))
        {

        }

        public DialogueTaggedText(TagType tag, IDialogueText dialogueText = null)
        {
            this.Tag = tag;
            this.Text = dialogueText;
        }

        public IDialogueText Text
        {
            get;
            private set;
        }
        public TagType Tag
        {
            get;
            private set;
        }

        public void AddText(string text)
        {
            if (this.Text == null)
            {
                this.Text = new DialogueText(text);
            }   
            else
            {
                this.Text.AddText(text);
            }   
        }

        public void AddText(IDialogueText dialogueText)
        {
            if (this.Text == null)
            {
                this.Text = dialogueText;
            }   
            else
            {
                this.Text.AddText(dialogueText);
            }
        }

        public override string ToString() => this.FullText;

        public IEnumerable<string> Parse() => Tag.Parse(Text.Parse); // Parsear acorde con el tag

        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según si contiene o no tags.
        /// <para>Opcionalmente, puedes añadir un <paramref name="logger"/> para que te reporte las excepciones internas que se puedan producir (mediante <see cref="ParsingException"/>).</para>
        /// </summary>
        /// <param name="text">El texto de entrada.</param>
        /// <param name="format">El formato de Tag que deseas que analice dentro del texto.</param>
        /// <returns></returns>
        public static IDialogueText AnalyzeText(string text, TagFormat format)
        {
            IDialogueText resultDialogueText = null;

            string textBeingAnalyzed = text;
            int currentIndex = 0;

            // Mientras quede texto por analizar
            while (textBeingAnalyzed.Length > 0)
            {
                int nextIndex = currentIndex;
                int indexOfTagInit = 0;

                try
                {
                    // Extraes siguiente tag que haya
                    TagOption tag = format.Extract(textBeingAnalyzed, out indexOfTagInit, out int _, out string remainingTextAfterStart);

                    if (tag != null)
                    {
                        nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfTagInit) + tag.Text().Length;
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

                        if (tag.Position == TagOptionPosition.Start)
                        {
                            string textSearchingForEnd = remainingTextAfterStart;
                            string taggedText = null, remainingTextAfterEnd = null;
                            TagOption endTag = null;
                            while (taggedText == null && textSearchingForEnd.Length > 0)
                            {
                                endTag = format.Extract(textSearchingForEnd, out int indexOfEndTagInit, out int _, out remainingTextAfterEnd);
                                if (endTag != null)
                                {
                                    if (TagOption.Matches(tag, endTag))
                                    {
                                        taggedText = remainingTextAfterStart.Substring(0, remainingTextAfterStart.Length - remainingTextAfterEnd.Length - endTag.Text().Length);
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
                                DialogueTaggedText dialogueTaggedText = new DialogueTaggedText(new TagType(tag, endTag), AnalyzeText(taggedText, format));
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
                                    resultDialogueText.AddText(dialogueTaggedText);
                                }
                            }
                        }
                        else
                        {
                            throw new TagException.EndTagBeforeStartException(tag, currentIndex);
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
                catch (ParsingException ex)
                {
                    Console.WriteLine(ex.Message);

                    nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfTagInit) + 1;

                    // Go to the next portion of the text (Skip the exception source)
                    textBeingAnalyzed = textBeingAnalyzed.Substring(nextIndex);
                    currentIndex = nextIndex;
                }
            }

            return resultDialogueText;
        }

        private string FullText => Tag?.GetTaggedText(Text.ToString());
    }
}