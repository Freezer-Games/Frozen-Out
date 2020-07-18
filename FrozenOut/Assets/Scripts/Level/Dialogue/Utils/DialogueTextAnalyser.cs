using Scripts.Level.Dialogue.Utils.Tag;

namespace Scripts.Level.Dialogue.Utils
{
    public class DialogueTextAnalyser
    {
        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según el tipo de <see cref="IDialogueText"/> que es (si contiene o no tags, etc.)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDialogueText AnalyseText(string text)
        {
            IDialogueText resultDialogueText;

            if (TagFormat.RichTextTagFormat.HasAnyTags(text))
            {
                resultDialogueText = AnalyseTaggedText(text, TagFormat.RichTextTagFormat);
            }
            else
            {
                resultDialogueText = new DialogueText(text);
            }

            return resultDialogueText;
        }

        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según si contiene o no tags.
        /// </summary>
        /// <param name="text">El texto de entrada.</param>
        /// <param name="format">El formato de Tag que deseas que analice dentro del texto.</param>
        /// <returns></returns>
        private static IDialogueText AnalyseTaggedText(string text, TagFormat format)
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

                    if (tag != null) // Aún hay tags en el texto
                    {
                        nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfTagInit) + tag.Text().Length;

                        if (indexOfTagInit > 0) // Si hay tag inicial, si no hay es porque no existe inicio porque se ha quitado en iteraciones anteriores
                        {
                            // Coger el texto antes de que empiece el tag y añadirlo a resultDialogueText
                            string textBeforeTag = textBeingAnalyzed.Substring(0, indexOfTagInit);
                            if (resultDialogueText == null)
                            {
                                // El texto original es un ComplexDialogueText
                                resultDialogueText = new ComplexDialogueText(textBeforeTag);
                            }
                            else
                            {
                                // Añadir el texto al ComplexDialogueText
                                resultDialogueText.AddText(textBeforeTag);
                            }
                        }

                        if (tag.Position == TagOptionPosition.Start) // si el tag es del principio
                        {
                            string textSearchingForEnd = remainingTextAfterStart;
                            string taggedText = null;
                            string remainingTextAfterEnd = null;
                            TagOption endTag = null;
                            while (taggedText == null && textSearchingForEnd.Length > 0) // Mientras haya texto
                            {
                                // Extraer el siguiente tag, que deberia ser el tag de cierre
                                endTag = format.Extract(textSearchingForEnd, out int indexOfEndTagInit, out int _, out remainingTextAfterEnd);
                                if (endTag != null) // Si el endTag se ha encontrado
                                {
                                    if (TagOption.Matches(tag, endTag)) // Si es el endTag del tag actual (soporte para nested tags)
                                    {
                                        taggedText = remainingTextAfterStart.Substring(0, remainingTextAfterStart.Length - remainingTextAfterEnd.Length - endTag.Text().Length);
                                        textBeingAnalyzed = remainingTextAfterEnd;
                                    }
                                    else
                                    {
                                        // Actualizar texto para encontrar el endTag del tag actual
                                        textSearchingForEnd = remainingTextAfterEnd;
                                    }
                                }
                            }

                            if (taggedText == null) // Si no se ha encontrado tag de cierre para el tag actual
                            {
                                throw new TagException.StartTagWithoutEndException(tag, indexOfTagInit);
                            }
                            else
                            {
                                // Crear DialogueTaggedText a partir de este tag, porque ya sabemos cuando termina
                                // Llamada recursiva a AnalyzeText para encontrar el texto interior (y soportar posibles tags nesteados)
                                DialogueTaggedText dialogueTaggedText = new DialogueTaggedText(new TagType(tag, endTag), AnalyseTaggedText(taggedText, format));
                                if (resultDialogueText == null)
                                {
                                    if (remainingTextAfterEnd != null && remainingTextAfterEnd.Length > 0) // Si hay más texto luego
                                    {
                                        // El texto original es un ComplexDialogueText
                                        resultDialogueText = new ComplexDialogueText(dialogueTaggedText);
                                    }
                                    else // No existe más texto luego
                                    {
                                        // El texto original es un DialogueTaggedText
                                        resultDialogueText = dialogueTaggedText;
                                    }
                                }
                                else
                                {
                                    // Añadir el texto al ComplexDialogueText
                                    resultDialogueText.AddText(dialogueTaggedText);
                                }
                            }
                        }
                        else
                        {
                            throw new TagException.EndTagBeforeStartException(tag, currentIndex);
                        }
                    }
                    else // No se ha encontrado ningun tag más
                    {
                        if (resultDialogueText == null)
                        {
                            // El texto original es un DialogueText
                            resultDialogueText = new DialogueText(textBeingAnalyzed);
                        }
                        else
                        {
                            // Añadir el texto al ComplexDialogueText
                            resultDialogueText.AddText(textBeingAnalyzed);
                        }

                        return resultDialogueText;
                    }
                }
                catch (ParsingException)
                {
                    nextIndex = (text.Length - textBeingAnalyzed.Length + indexOfTagInit) + 1;

                    // Go to the next portion of the text (Skip the exception source)
                    textBeingAnalyzed = textBeingAnalyzed.Substring(nextIndex);
                    currentIndex = nextIndex;
                }
            }

            return resultDialogueText;
        }
    }
}