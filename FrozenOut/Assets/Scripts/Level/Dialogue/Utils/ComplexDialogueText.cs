using System;
using System.Collections.Generic;
using System.Linq;

using Scripts.Level.Dialogue.Utils.Tag;

namespace Scripts.Level.Dialogue.Utils
{
    /// <summary>
    /// Representa un texto de diálogo que esta compuesto de más texto
    /// </summary>
    public class ComplexDialogueText : IDialogueText
    {
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

        public List<IDialogueText> Texts
        {
            get;
            private set;
        }

        public void AddText(string text)
        {
            AddText(new DialogueText(text));
        }

        public void AddText(IDialogueText dialogueText)
        {
            Texts.Add(dialogueText);
        }

        /// <summary>
        /// Obtiene el texto de todos los <see cref="IDialogueText"/>, y lo devuelve letra a letra de forma acumulada.
        /// <para>Si el texto tiene tags (<see cref="DialogueTaggedText"/>),
        /// envolverá el texto en el tag para que el usuario nunca vea los caracteres asociados al mismo (los cuales no forman parte del texto).</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseAccumulated()
        {
            string currentTotalText = "";
            foreach (IDialogueText text in Texts)
            {
                string currentText = "";
                foreach (string nextTextAccumulated in text.ParseAccumulated())
                {
                    currentText = nextTextAccumulated;
                    yield return currentTotalText + nextTextAccumulated;
                }
                currentTotalText += currentText;
            }
        }

        /// <summary>
        /// Obtiene el texto de todos los <see cref="IDialogueText"/>, y lo devuelve letra a letra sin acumular el texto previo.
        /// <para>Si el texto tiene tags (<see cref="DialogueTaggedText"/>),
        /// envolverá el carácter en el tag para que el usuario nunca vea los carácteres asociados al mismo (los cuales no forman parte del texto).</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseSingle()
        {
            foreach(IDialogueText text in Texts)
            {
                foreach(string nextLetter in text.ParseSingle())
                {
                    yield return nextLetter;
                }
            }
        }

        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según el tipo de <see cref="IDialogueText"/> que es (si contiene o no tags, etc.)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDialogueText AnalyzeText(string text)
        {
            IDialogueText resultDialogueText;

            if (TagFormat.RichTextTagFormat.HasAnyTags(text))
            {
                resultDialogueText = DialogueTaggedText.AnalyzeText(text, TagFormat.RichTextTagFormat);
            }
            else
            {
                resultDialogueText = new DialogueText(text);
            }

            return resultDialogueText;
        }

        public string ToStringClean() => Texts.Aggregate("", (completeText, text) => completeText + text.ToStringClean());
        public string ToStringFull() => Texts.Aggregate("", (completeText, text) => completeText + text.ToStringFull());
    }
}