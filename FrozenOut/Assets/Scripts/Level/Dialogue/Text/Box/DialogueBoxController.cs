using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Scripts.Level.Dialogue.Text
{
    public class DialogueBoxController : MonoBehaviour
    {
        public GameObject LinePrefab;
        public GameObject WordPrefab;
        public GameObject LetterPrefab;

        private GameObject CurrentLine;
        private GameObject CurrentWord;

        private float CurrentLineWidth => CurrentLine != null ? CurrentLine.GetComponent<RectTransform>().rect.width : 0.0f;
        private float MaxWidthPerLine;

        private ICollection<Animator> TextAnimators;
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

        private void Awake()
        {
            RectTransform holderTransform = GetComponent<RectTransform>();
            MaxWidthPerLine = holderTransform.rect.width - 200.0f;

            TextAnimators = new List<Animator>();
        }

        public void Clear()
        {
            StopAllCoroutines();
            if (TextAnimators != null)
            {
                TextAnimators.Clear();
            }
            else
            {
                TextAnimators = new List<Animator>();
            }
            CurrentLine = null;
            CurrentWord = null;

            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void SetText(string letter, TextStyle style)
        {
            bool IsSpace = letter.Contains(" ");
            if (CurrentLineWidth >= MaxWidthPerLine && IsSpace)
            {
                CreateLine();
            }

            GameObject currentLetter;
            if (IsSpace)
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

            if (style.Effect == TextStyle.TextEffect.Interrupted)
            {
                letter = RandomGarbageLetter(letter);
            }

            UnityEngine.UI.Text textHolder = currentLetter.GetComponent<UnityEngine.UI.Text>();
            textHolder.text = letter;
            textHolder.font = style.Font;
            textHolder.fontSize = style.Size;

            UnityEngine.UI.Text textComponent = currentLetter.GetComponentsInChildren<UnityEngine.UI.Text>().Last(); //Excluir holder de antes
            textComponent.text = letter;
            textComponent.font = style.Font;
            textComponent.fontSize = style.Size;
            textComponent.color = style.Colour;

            if (style.Effect != TextStyle.TextEffect.None && style.Effect != TextStyle.TextEffect.Interrupted)
            {
                string animation = MapAnimationToString(style.Effect);
                AnimateSequential(animation);
            }
        }

        private void CreateLine()
        {
            GameObject lineObject = GameObject.Instantiate(LinePrefab, transform);
            CurrentLine = lineObject;
        }

        private void CreateWord()
        {
            if (CurrentLine == null)
            {
                CreateLine();
            }

            GameObject wordObject = GameObject.Instantiate(WordPrefab, CurrentLine.transform);
            CurrentWord = wordObject;
        }

        private GameObject CreateLetter()
        {
            if (CurrentWord == null)
            {
                CreateWord();
            }

            GameObject letterObject = GameObject.Instantiate(LetterPrefab, CurrentWord.transform);

            return letterObject;
        }

        private GameObject CreateSeparator()
        {
            if (CurrentLine == null)
            {
                CreateLine();
            }

            GameObject separatorObject = GameObject.Instantiate(LetterPrefab, CurrentLine.transform);

            return separatorObject;
        }

        private string RandomGarbageLetter(string originalLetter)
        {
            string letter = originalLetter;

            bool useRandom = UnityEngine.Random.Range(0, 11) > 7; // 30% of random
            if (useRandom)
            {
                int randomIndex = UnityEngine.Random.Range(0, GarbageLetters.Count());

                letter = GarbageLetters.ElementAt(randomIndex);
            }

            return letter;
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

            while (true)
            {
                foreach (Animator animator in TextAnimators)
                {
                    animator.SetTrigger(animationString);

                    yield return new WaitForSeconds(BetweenDelay);
                }

                yield return new WaitForSeconds(EndDelay);
            }
        }
    }
}