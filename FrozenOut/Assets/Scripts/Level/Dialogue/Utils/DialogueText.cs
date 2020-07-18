using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Level.Dialogue.Utils
{
    /// <summary>
    /// Represents a simple Dialogue with a text
    /// </summary>
    public class DialogueText : IDialogueText
    {
        public DialogueText(string text)
        {
            this.Text = text;
        }

        public string Text
        {
            get;
            private set;
        }

        public void AddText(string text)
        {
            this.Text += text;
        }

        public void AddText(IDialogueText dialogueText)
        {
            this.Text += dialogueText.ToStringFull();
        }

        /// <summary>
        /// Devuelve el texto letra a letra de forma acumulada
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseAccumulated()
        {
            string currentText = "";
            foreach (char letter in Text)
            {
                currentText += letter;
                yield return currentText;
            }
        }

        /// <summary>
        /// Devuelve el texto letra a letra sin acumular
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ParseSingle()
        {
            foreach (char letter in Text)
            {
                yield return letter.ToString();
            }
        }

        public string ToStringClean() => this.Text;
        public string ToStringFull() => this.Text;


        /*private readonly List<char> GarbageLetters = new List<char>()
        {
            '#',
            '$',
            '?',
            '@',
            '-',
            '*',
            '&',
            '%'
        };

        private string RandomGarbageWord(string word)
        {
            char[] letters = word.ToCharArray();

            Random random = new Random();
            for (int index = 0; index < letters.Count(); index++)
            {
                bool useRandom = random.Next(0, 11) > 7; // 30% of random

                int randomIndex = random.Next(0, GarbageLetters.Count());

                if (useRandom)
                {
                    letters[index] = GarbageLetters.ElementAt(randomIndex);
                }
            }

            return new string(letters);
        }*/
    }
}