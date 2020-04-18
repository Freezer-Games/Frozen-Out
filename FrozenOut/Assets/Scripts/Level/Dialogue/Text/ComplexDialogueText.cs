using System;
using System.Collections.Generic;
using System.Linq;

using Scripts.Level.Dialogue.Text.Tag;

namespace Scripts.Level.Dialogue.Text
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

        public override string ToString() => FullText;

        public void AddText(string text)
        {
            this.AddText(new DialogueText(text));
        }

        public void AddText(IDialogueText dialogueText)
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

        private string FullText => Texts.Aggregate("", (fullText, text) => fullText + text.ToString());
    }
}