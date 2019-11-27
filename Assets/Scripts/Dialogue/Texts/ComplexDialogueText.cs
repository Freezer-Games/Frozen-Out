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
            IDialogueText resultDialogueText;

            if (TagFormat.RichTextTagFormat.HasAnyTags(text))
            {
                resultDialogueText = DialogueTaggedText.AnalyzeText(text, TagFormat.RichTextTagFormat, logger);
            }
            else
            {
                resultDialogueText = new DialogueText(text);
            }

            return resultDialogueText;
        }
    }
}
