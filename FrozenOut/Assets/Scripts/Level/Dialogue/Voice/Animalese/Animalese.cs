using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Level.Dialogue.Voice.Animalese
{
    public class Animalese : VoiceManager
    {
        public AudioSource AudioSource;

        public AudioClip A;
        public AudioClip B;
        public AudioClip C;
        public AudioClip CH;
        public AudioClip D;
        public AudioClip E;
        public AudioClip EE;
        public AudioClip F;
        public AudioClip G;
        public AudioClip H;
        public AudioClip I;
        public AudioClip J;
        public AudioClip K;
        public AudioClip L;
        public AudioClip M;
        public AudioClip N;
        public AudioClip O;
        public AudioClip OO;
        public AudioClip P;
        public AudioClip Q;
        public AudioClip R;
        public AudioClip S;
        public AudioClip SH;
        public AudioClip T;
        public AudioClip TH;
        public AudioClip U;
        public AudioClip V;
        public AudioClip W;
        public AudioClip X;
        public AudioClip Y;
        public AudioClip Z;

        public AudioClip Space;
        public AudioClip Period;

        private string[] CurrentSentences;

        private VoiceStyle CurrentStyle;

        public override void Open()
        {
            // TODO
        }

        public override void Close()
        {
            Stop();
        }

        public override void SetStyle(VoiceStyle style)
        {
            CurrentStyle = style;

            AudioSource.volume = CurrentStyle.Volume;
        }

        public override void Speak(string dialogue)
        {
            Stop();

            char lastCharacter = dialogue.Last();
            if (!lastCharacter.Equals('.') && !lastCharacter.Equals('?') && !lastCharacter.Equals('!') && !lastCharacter.Equals(',') && !lastCharacter.Equals(':') && !lastCharacter.Equals(';'))
            {
                // Añade punto final
                dialogue += ".";
            }

            // Divide el texto en frases independientes (las que acaban con un signo de puntuacion)
            CurrentSentences = Regex.Split(dialogue, "([.?!,:;])");
            
            StartCoroutine(SpeakSentences());
        }

        public override void Speak(char newDialogueLetter)
        {
            Stop();

            PlayLetter(newDialogueLetter);
        }

        private void Stop()
        {
            StopCoroutine(SpeakSentences());
            AudioSource.Stop();
        }

        private IEnumerator SpeakSentences()
        {
            string currentSentence;
            char currentPunctuation;

            for (int indexSentence = 0; indexSentence < CurrentSentences.Length - 1; indexSentence+=2)
            {
                currentSentence = CurrentSentences.ElementAt(indexSentence).ToLower();
                currentPunctuation = CurrentSentences.ElementAtOrDefault(indexSentence + 1).First();

                int indexLetter = 0;
                while (indexLetter < currentSentence.Length)
                {
                    char currentLetter = currentSentence.ElementAt(indexLetter);
                    char nextLetter = currentSentence.ElementAtOrDefault(indexLetter);
                    
                    // TODO
                    //this.Enunciate(punctuation, j, sentLength);
                    int lettersUsed = PlayLetter(currentLetter, nextLetter);
                    indexLetter += lettersUsed;

                    yield return new WaitForSeconds(CurrentStyle.Delay); //Wait for next letter
                }

                yield return new WaitForSeconds(0.5f); //Wait for next sentence
            }
            yield break;
        }

        private int PlayLetter(char letter, char? nextLetter = null)
        {
            int lettersUsed = 1;

            bool existsNextLetter = nextLetter != null;
            switch (letter)
            {
                case 'a':
                    PlayClip(A);
                    break;
                case 'b':
                    PlayClip(B);
                    break;
                case 'c':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('h')) //ch
                        {
                            PlayClip(CH);
                            lettersUsed = 2;
                            break;
                        }
                        else if (nextLetter.Equals('k')) //ck
                        {
                            PlayClip(K);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(K);
                    break;
                case 'd':
                    PlayClip(D);
                    break;
                case 'e':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('e') || nextLetter.Equals('y')) //ee, ey
                        {
                            PlayClip(EE);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(E);
                    break;
                case 'f':
                    PlayClip(F);
                    break;
                case 'g':
                    PlayClip(G);
                    break;
                case 'h':
                    PlayClip(H);
                    break;
                case 'i':
                    PlayClip(I);
                    break;
                case 'j':
                    PlayClip(J);
                    break;
                case 'k':
                    PlayClip(K);
                    break;
                case 'l':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('l')) //ll
                        {
                            PlayClip(L);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(L);
                    break;
                case 'm':
                    PlayClip(M);
                    break;
                case 'n':
                    PlayClip(N);
                    break;
                case 'o':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('o')) //oo
                        {
                            PlayClip(OO);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(O);
                    break;
                case 'p':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('h')) //ph
                        {
                            PlayClip(F);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(P);
                    break;
                case 'q':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('u')) //qu
                        {
                            PlayClip(W);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(K);
                    break;
                case 'r':
                    PlayClip(R);
                    break;
                case 's':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('h')) //sh
                        {
                            PlayClip(SH);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(S);
                    break;
                case 't':
                    if (existsNextLetter)
                    {
                        if (nextLetter.Equals('h')) //th
                        {
                            PlayClip(TH);
                            lettersUsed = 2;
                            break;
                        }
                    }
                    PlayClip(T);
                    break;
                case 'u':
                    PlayClip(U);
                    break;
                case 'v':
                    PlayClip(V);
                    break;
                case 'w':
                    PlayClip(W);
                    break;
                case 'x':
                    PlayClip(X);
                    break;
                case 'y':
                    PlayClip(Y);
                    break;
                case 'z':
                    PlayClip(Z);
                    break;
                case ' ':
                    PlayClip(Space);
                    break;
                case '.':
                    PlayClip(Period);
                    break;
                default:
                    PlayClip(GetRandomLetter());
                    break;
            }

            return lettersUsed;
        }

        private AudioClip GetRandomLetter()
        {
            // TODO
            return A;
        }

        private void PlayClip(AudioClip letter)
        {
            AudioSource.PlayOneShot(letter);
            //AudioSource.PlayClipAtPoint(letter, CurrentPoint.transform.position);
        }

        /*private void Enunciate(char punc, int currentChar, int totalChars)
        {
            switch (punc)
            {
                case '!':
                    currentPitch = this.Pitch + 10f;
                    currentVolume = Mathf.Clamp(this.Volume + 30f, 0.0f, 100f);
                    break;
                case '?':
                    currentPitch = this.Pitch + (10 - Mathf.Clamp(totalChars - currentChar, 0, 10)) * 2;
                    currentVolume = this.Volume;
                    break;
                default:
                    currentPitch = this.Pitch;
                    currentVolume = this.Volume;
                    break;
            }
        }*/
    }
}