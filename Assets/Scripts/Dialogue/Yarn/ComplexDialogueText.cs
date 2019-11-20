using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dialogue.Yarn
{
    public class ComplexDialogueText
    {
        public List<DialogueText> Texts { get; set; }

        private string FullText => Texts.Aggregate("", (fullText, text) => fullText + text.ToString());

        public ComplexDialogueText()
        {

        }

        public ComplexDialogueText(List<DialogueText> texts)
        {
            Texts = texts;
        }

        public ComplexDialogueText(params DialogueText[] texts)
        {
            Texts = new List<DialogueText>(texts);
        }

        /// <summary>
        /// Si los <paramref name="texts"/> no tienen ningún tag, se pueden pasar directamente como <see cref="string"/>, y este constructor los convertirá a <see cref="DialogueText"/> por ti.
        /// </summary>
        /// <param name="texts"></param>
        public ComplexDialogueText(params string[] texts)
        {
            Texts = new List<DialogueText>(texts.Select(text => new DialogueText(text)));
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
        /// Obtiene el texto de todos los <see cref="DialogueText"/>, y lo devuelve letra a letra.
        /// <para>En este método, no importa si el texto tiene tags (<see cref="DialogueTaggedText"/>), devolverá cada carácter uno por uno.</para>
        /// <para>Para un comportamiento más útil para mostrar en elementos de la UI, es mejor utilizar <see cref="ParseInBuilder(StringBuilder)"/>.</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<char> Parse()
        {
            foreach (DialogueText text in Texts)
            {
                foreach (char letter in text.Parse())
                {
                    yield return letter;
                }
            }
        }

        public override string ToString() => FullText;


        /// <summary>
        /// Analiza el <paramref name="text"/> indicado, y lo clasifica según el tipo de <see cref="DialogueText"/> que es (si contiene o no tags, etc.)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ComplexDialogueText AnalyzeText(string text)
        {
            string textBeingAnalyzed = text;
            ComplexDialogueText complexDialogueText = new ComplexDialogueText();

            while (text.Length > 0)
            {            
                int indexOfNextTagInit = textBeingAnalyzed.IndexOf(DialogueTaggedText.TAG_SEPARATOR_INIT);
                if (indexOfNextTagInit >= 0)
                {
                    string textBeforeTag = text.Substring(0, indexOfNextTagInit);
                    complexDialogueText.Texts.Add(new DialogueText(textBeforeTag));

                    DialogueTaggedText.ExtractTag(textBeingAnalyzed, indexOfNextTagInit, out string _, out string tag, out TagOptionPosition position, out string remainingText);


                }
                else
                {
                    complexDialogueText.Texts.Add(new DialogueText(textBeingAnalyzed));
                }
            }

            return complexDialogueText;
        }
    }
}
