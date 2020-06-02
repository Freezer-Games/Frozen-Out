using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class TextStyleController : MonoBehaviour
    {
        public GameObject LinePrefab;
        public GameObject WordPrefab;
        public GameObject LetterPrefab;
        
        private GameObject CurrentLine;
        private GameObject CurrentWord;

        private int CurrentLineLetters;
        private const int MaxLettersPerLine = 35;

        private ICollection<Animator> TextAnimators;
        private TextStyle CurrentStyle;

        private const float BetweenDelay = 0.15f;
        private const float EndDelay = 0.5f;

        private List<string> GarbageLetters = new List<string>()
        {
            "#",
            "$",
            "?",
            "@",
            "-",
            "*",
            "&",
            "%"
        };

        void Awake()
        {
            TextAnimators = new List<Animator>();
        }

        public void Clear()
        {
            StopAllCoroutines();
            if(TextAnimators != null)
            {
                TextAnimators.Clear();
            }
            else
            {
                TextAnimators = new List<Animator>();
            }
            CurrentLine = null;
            CurrentWord = null;
            CurrentLineLetters = 0;

            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void SetStyle(TextStyle style)
        {
            Clear();
            CurrentStyle = style;
        }

        public void SetTextLine(string dialogue)
        {
            Clear();
            foreach(char letter in dialogue)
            {
                SetTextLetter(letter.ToString());
            }
        }

        public void SetTextLetter(string letter)
        {
            SetText(letter, CurrentStyle);
        }

        private void SetText(string letter, TextStyle style)
        {
            bool IsSpace = letter.Contains(" ");//letter.Equals(" ");
            if(CurrentLineLetters >= MaxLettersPerLine && IsSpace)
            {
                CreateLine();
                CurrentLineLetters = 0;
            }
            CurrentLineLetters++;

            GameObject currentLetter;
            if(IsSpace)
            {
                currentLetter = CreateSeparator();
                CreateWord();
            }
            else
            {
                currentLetter = CreateLetter();

                Animator textAnimator = currentLetter.GetComponentInChildren<Animator>();
                TextAnimators.Add(textAnimator);
            }

            if(style.Effect == TextStyle.TextEffect.Interrupted)
            {
                letter = RandomGarbageLetter(letter);
            }

            UnityEngine.UI.Text textHolder = currentLetter.GetComponent<UnityEngine.UI.Text>();
            textHolder.text = letter;
            textHolder.font = style.Font;
            textHolder.fontSize = style.Size;

            UnityEngine.UI.Text textComponent = currentLetter.GetComponentsInChildren<UnityEngine.UI.Text>().Last(); //Excluir holder de antes
            textComponent.text = letter;
            SetStyle(textComponent, style);

            if(style.Effect != TextStyle.TextEffect.None && style.Effect != TextStyle.TextEffect.Interrupted)
            {
                string animation = MapAnimationToString(style.Effect);
                AnimateSequential(animation);
            }
        }

        private string RandomGarbageLetter(string originalLetter)
        {
            string letter = originalLetter;

            bool useRandom = Random.Range(0, 11) > 7; // 30% of random
            if(useRandom)
            {
                int randomIndex = Random.Range(0, GarbageLetters.Count());

                letter = GarbageLetters.ElementAt(randomIndex);
            }

            return letter;
        }

        private void CreateLine()
        {
            GameObject lineObject = GameObject.Instantiate(LinePrefab, this.transform);
            CurrentLine = lineObject;
        }

        private void CreateWord()
        {
            if(CurrentLine == null)
            {
                CreateLine();
            }

            GameObject wordObject = GameObject.Instantiate(WordPrefab, CurrentLine.transform);
            CurrentWord = wordObject;
        }

        private GameObject CreateSeparator()
        {
            if(CurrentLine == null)
            {
                CreateLine();
            }

            GameObject separatorObject = GameObject.Instantiate(LetterPrefab, CurrentLine.transform);

            return separatorObject;
        }

        private GameObject CreateLetter()
        {
            if(CurrentWord == null)
            {
                CreateWord();
            }

            GameObject letterObject = GameObject.Instantiate(LetterPrefab, CurrentWord.transform);

            return letterObject;
        }

        private void SetStyle(UnityEngine.UI.Text text, TextStyle style)
        {
            text.font = style.Font;
            text.fontSize = style.Size;
            text.color = style.Colour;
        }

        private void AnimateSequential(string animationString)
        {
            StopAllCoroutines();
            StartCoroutine(DoAnimateSequential(animationString));
        }

        private string MapAnimationToString(TextStyle.TextEffect textEffect)
        {
            string animationString = "";

            switch (textEffect)
            {
                case TextStyle.TextEffect.Jumping:
                    animationString = "Jump";
                    break;
                case TextStyle.TextEffect.Fading:
                    animationString = "Fade";
                    break;
                case TextStyle.TextEffect.Highlighted:
                    animationString = "Highlight";
                    break;
            }

            return animationString;
        }

        private IEnumerator DoAnimateSequential(string animationString)
        {
            TextAnimators.Last().SetTrigger(animationString);
            yield return new WaitForSeconds(EndDelay);

            while(true)
            {
                foreach(Animator animator in TextAnimators)
                {
                    animator.SetTrigger(animationString);

                    yield return new WaitForSeconds(BetweenDelay);
                }

                yield return new WaitForSeconds(EndDelay);
            }
        }
    }
}