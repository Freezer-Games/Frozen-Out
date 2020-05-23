/*using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Level.Dialogue.Voice.WWise
{
    public class DIAL_Engine : MonoBehaviour
    {
        public string dialogue = "...";
        private float currentVolume = 50f;
        private float currentPitch = 50f;
        private float virtualWidth = 960f;
        private float virtualHeight = 540f;
        public GameObject soundSource;
        public GUISkin customGUISkin;
        public GameObject pointer;
        public NPC npc;
        private string[] sentences;
        private string sent;
        private string punc;
        private string npcName;
        private uint bankID;

        private void Start()
        {
            int num = (int)AkSoundEngine.LoadBank("DIAL_Bank.bnk", -1, out bankID);
            pointer.transform.position = new Vector3(npc.transform.position.x - 1.9f, pointer.transform.position.y, pointer.transform.position.z);
        }

        private void Update()
        {
            int num1 = (int)AkSoundEngine.SetRTPCValue("Volume", currentVolume);
            int num2 = (int)AkSoundEngine.SetRTPCValue("Pitch", currentPitch);
            int num3 = (int)AkSoundEngine.SetRTPCValue("Tone", npc.tone);
            int num4 = (int)AkSoundEngine.SetRTPCValue("Gender", npc.gender);
            if (!Input.GetMouseButtonDown(0))
                return;
            RaycastHit hitInfo;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (!(hitInfo.collider.gameObject.tag == "NPCs") || !(npc != hitInfo.collider.gameObject.GetComponent<NPC>()))
                return;
            if (npc.isSpeaking)
            {
                npc.isSpeaking = false;
                npc.isActive = false;
                npc = hitInfo.collider.gameObject.GetComponent<NPC>();
                npc.isSpeaking = true;
                npc.isActive = true;
                UnityEngine.Debug.Log("Popcorn " + npc + "!");
            }
            else
            {
                npc.isActive = false;
                npc = hitInfo.collider.gameObject.GetComponent<NPC>();
                npc.isActive = true;
                UnityEngine.Debug.Log(npc);
            }
            pointer.transform.position = new Vector3(npc.transform.position.x - 1.9f, pointer.transform.position.y, pointer.transform.position.z);
            string name = npc.name;
            if (name == null)
                return;

            // ISSUE: reference to a compiler-generated field
            if (DIAL_Engine.\u003C\u003Ef__switch\u0024map0 == null)
            {
                // ISSUE: reference to a compiler-generated field
                DIAL_Engine.\u003C\u003Ef__switch\u0024map0 = new Dictionary<string, int>(1)
                {
                    {
                        "NPC_blue",
                        0
                    }
                };
            }

            int num5;

            // ISSUE: reference to a compiler-generated field
            if (!DIAL_Engine.\u003C\u003Ef__switch\u0024map0.TryGetValue(name, out num5) || num5 != 0)
                return;

            npcName = "Bleu";
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeight, 1f));
            GUI.skin = customGUISkin;
            GUI.Label(new Rect(15f, 350f, 150f, 50f), "parameters - " + npc.name, "subtitle");
            GUI.Label(new Rect(10f, 500f, 150f, 50f), "Dialogue Engine (v1.0) © 2014 Zack Bogucki  -  www.zbogucki.com", "credits");
            dialogue = GUI.TextField(new Rect(95f, 45f, 745f, 70f), dialogue, 500);
            if (GUI.Button(new Rect(755f, 118f, 75f, 35f), "SPEAK") || Input.GetKeyDown("return"))
                Speak(dialogue);
            GUI.Label(new Rect(155f, 388f, 80f, 35f), "Pitch");
            npc.pitch = GUI.HorizontalSlider(new Rect(245f, 405f, 350f, 20f), npc.pitch, 20f, 65f);
            GUI.Label(new Rect(155f, 408f, 80f, 35f), "Volume");
            npc.volume = GUI.HorizontalSlider(new Rect(245f, 425f, 350f, 20f), npc.volume, 0.0f, 100f);
            GUI.Label(new Rect(155f, 428f, 80f, 35f), "Gender");
            npc.gender = GUI.HorizontalSlider(new Rect(245f, 445f, 350f, 20f), npc.gender, 0.0f, 100f);
            GUI.Label(new Rect(155f, 448f, 80f, 35f), "Tone");
            npc.tone = GUI.HorizontalSlider(new Rect(245f, 465f, 350f, 20f), npc.tone, 0.0f, 100f);
            GUI.Label(new Rect(155f, 468f, 80f, 35f), "Speed");
            npc.speed = GUI.HorizontalSlider(new Rect(245f, 485f, 350f, 20f), npc.speed, 0.0f, 100f);
            if (GUI.Button(new Rect(710f, 370f, 100f, 18f), "ALL DEFAULT", "defaultButton"))
            {
                npc.SendMessage("ResetAll");
                if (!npc.isSpeaking)
                    Speak("Default");
            }
            if (GUI.Button(new Rect(710f, 400f, 100f, 18f), "DEFAULT", "defaultButton"))
            {
                npc.SendMessage("ResetPitch");
                if (!npc.isSpeaking)
                    Speak("Default");
            }
            if (GUI.Button(new Rect(710f, 420f, 100f, 18f), "DEFAULT", "defaultButton"))
            {
                npc.SendMessage("ResetVolume");
                if (!npc.isSpeaking)
                    Speak("Default");
            }
            if (GUI.Button(new Rect(710f, 440f, 100f, 18f), "DEFAULT", "defaultButton"))
            {
                npc.SendMessage("ResetGender");
                if (!npc.isSpeaking)
                    Speak("Default");
            }
            if (GUI.Button(new Rect(710f, 460f, 100f, 18f), "DEFAULT", "defaultButton"))
            {
                npc.SendMessage("ResetTone");
                if (!npc.isSpeaking)
                    Speak("Default");
            }
            if (GUI.Button(new Rect(710f, 480f, 100f, 18f), "DEFAULT", "defaultButton"))
            {
                npc.SendMessage("ResetSpeed");
                if (!npc.isSpeaking)
                    Speak("Default");
            }
            if (GUI.Button(new Rect(605f, 370f, 100f, 18f), "ALL RANDOM", "randomButton"))
            {
                npc.pitch = Random.Range(20, 65);
                npc.volume = Random.Range(0, 100);
                npc.gender = Random.Range(0, 100);
                npc.tone = Random.Range(0, 100);
                npc.speed = Random.Range(0, 100);
                if (!npc.isSpeaking)
                    Speak("Random");
            }
            if (GUI.Button(new Rect(605f, 400f, 100f, 18f), "RANDOM", "randomButton"))
            {
                npc.pitch = Random.Range(20, 65);
                if (!npc.isSpeaking)
                    Speak("Random");
            }
            if (GUI.Button(new Rect(605f, 420f, 100f, 18f), "RANDOM", "randomButton"))
            {
                npc.volume = Random.Range(0, 100);
                if (!npc.isSpeaking)
                    Speak("Random");
            }
            if (GUI.Button(new Rect(605f, 440f, 100f, 18f), "RANDOM", "randomButton"))
            {
                npc.gender = Random.Range(0, 100);
                if (!npc.isSpeaking)
                    Speak("Random");
            }
            if (GUI.Button(new Rect(605f, 460f, 100f, 18f), "RANDOM", "randomButton"))
            {
                npc.tone = Random.Range(0, 100);
                if (!npc.isSpeaking)
                    Speak("Random");
            }
            if (GUI.Button(new Rect(605f, 480f, 100f, 18f), "RANDOM", "randomButton"))
            {
                npc.speed = Random.Range(0, 100);
                if (!npc.isSpeaking)
                    Speak("Random");
            }
            if (!GUI.Button(new Rect(910f, 10f, 32f, 32f), " ", "exitButton") && !Input.GetKey("escape"))
                return;
            Application.Quit();
        }

        private void Speak(string textIn)
        {
            char ch = textIn[textIn.Length - 1];
            if (!ch.Equals('.') && !ch.Equals('?') && !ch.Equals('!') && !ch.Equals(',') && !ch.Equals(':') && !ch.Equals(';'))
            {
                textIn += ".";
                UnityEngine.Debug.Log(textIn);
            }
            sentences = Regex.Split(textIn, "([.?!,:;])");
            if (npc.isSpeaking)
            {
                StopCoroutine("SpeakSentences");
                npc.isSpeaking = false;
            }
            StartCoroutine("SpeakSentences");
        }

        private IEnumerator SpeakSentences()
        {
            this.npc.isSpeaking = true;
            for (int i = 0; i < this.sentences.Length - 1; i++)
            {
                this.sent = this.sentences[i];
                i++;
                this.punc = this.sentences[i];
                UnityEngine.Debug.Log(this.sent + this.punc);
                this.sent = this.sent.ToLower();
                bool isLastChar = false;
                char punctuation = this.punc[0];
                int sentLength = this.sent.Length - 1;
                int j = 0;
                while (j < this.sent.Length)
                {
                    isLastChar = (j == sentLength);
                    this.Enunciate(punctuation, j, sentLength);
                    switch (this.sent[j])
                    {
                        case 'a':
                            AkSoundEngine.SetSwitch("letters", "a", this.soundSource);
                            break;
                        case 'b':
                            AkSoundEngine.SetSwitch("letters", "b", this.soundSource);
                            break;
                        case 'c':
                            if (!isLastChar)
                            {
                                if (this.sent[j + 1].Equals('h'))
                                {
                                    AkSoundEngine.SetSwitch("letters", "ch", this.soundSource);
                                    j++;
                                    break;
                                }
                                if (this.sent[j + 1].Equals('k'))
                                {
                                    j++;
                                }
                            }
                            goto IL_40A;
                        case 'd':
                            AkSoundEngine.SetSwitch("letters", "d", this.soundSource);
                            break;
                        case 'e':
                            goto IL_2C3;
                        case 'f':
                            AkSoundEngine.SetSwitch("letters", "f", this.soundSource);
                            break;
                        case 'g':
                            AkSoundEngine.SetSwitch("letters", "g", this.soundSource);
                            break;
                        case 'h':
                            AkSoundEngine.SetSwitch("letters", "h", this.soundSource);
                            break;
                        case 'i':
                            AkSoundEngine.SetSwitch("letters", "i", this.soundSource);
                            break;
                        case 'j':
                            AkSoundEngine.SetSwitch("letters", "j", this.soundSource);
                            break;
                        case 'k':
                            goto IL_40A;
                        case 'l':
                            AkSoundEngine.SetSwitch("letters", "l", this.soundSource);
                            break;
                        case 'm':
                            AkSoundEngine.SetSwitch("letters", "m", this.soundSource);
                            break;
                        case 'n':
                            AkSoundEngine.SetSwitch("letters", "n", this.soundSource);
                            break;
                        case 'o':
                            if (!isLastChar && this.sent[j + 1].Equals('o'))
                            {
                                AkSoundEngine.SetSwitch("letters", "oo", this.soundSource);
                                j++;
                            }
                            else
                            {
                                AkSoundEngine.SetSwitch("letters", "o", this.soundSource);
                            }
                            break;
                        case 'p':
                            if (!isLastChar && this.sent[j + 1].Equals('h'))
                            {
                                AkSoundEngine.SetSwitch("letters", "f", this.soundSource);
                                j++;
                            }
                            else
                            {
                                AkSoundEngine.SetSwitch("letters", "p", this.soundSource);
                            }
                            break;
                        case 'q':
                            goto IL_40A;
                        case 'r':
                            AkSoundEngine.SetSwitch("letters", "r", this.soundSource);
                            break;
                        case 's':
                            goto IL_5DC;
                        case 't':
                            if (!isLastChar && this.sent[j + 1].Equals('h'))
                            {
                                AkSoundEngine.SetSwitch("letters", "th", this.soundSource);
                                j++;
                            }
                            else
                            {
                                AkSoundEngine.SetSwitch("letters", "t", this.soundSource);
                            }
                            break;
                        case 'u':
                            if (j != 0 && this.sent[j - 1].Equals('q'))
                            {
                                goto IL_756;
                            }
                            AkSoundEngine.SetSwitch("letters", "u", this.soundSource);
                            break;
                        case 'v':
                            AkSoundEngine.SetSwitch("letters", "v", this.soundSource);
                            break;
                        case 'w':
                            goto IL_756;
                        case 'x':
                            goto IL_40A;
                        case 'y':
                            if (isLastChar)
                            {
                                goto IL_2C3;
                            }
                            if (!isLastChar && (this.sent[j + 1].Equals(' ') || this.sent[j + 1].Equals(',')))
                            {
                                goto IL_2C3;
                            }
                            AkSoundEngine.SetSwitch("letters", "y", this.soundSource);
                            break;
                        case 'z':
                            AkSoundEngine.SetSwitch("letters", "z", this.soundSource);
                            break;
                        default:
                            AkSoundEngine.SetSwitch("letters", "random", this.soundSource);
                            break;
                    }
                IL_84B:
                    if (this.sent[j] != '-' && this.sent[j] != '\'')
                    {
                        if (this.sent[j] != ' ')
                        {
                            AkSoundEngine.PostEvent("Speak", this.soundSource);
                            yield return new WaitForSeconds((5f - this.npc.speed * 0.0385f) / 60f);
                        }
                        else
                        {
                            yield return new WaitForSeconds((5f - this.npc.speed * 0.0385f) / 180f);
                        }
                    }
                    j++;
                    continue;
                IL_2C3:
                    if (!isLastChar && (this.sent[j + 1].Equals('e') || this.sent[j].Equals('y')))
                    {
                        AkSoundEngine.SetSwitch("letters", "ee", this.soundSource);
                        j++;
                        goto IL_84B;
                    }
                    AkSoundEngine.SetSwitch("letters", "e", this.soundSource);
                    goto IL_84B;
                IL_40A:
                    AkSoundEngine.SetSwitch("letters", "k", this.soundSource);
                    if (!this.sent[j].Equals('x'))
                    {
                        goto IL_84B;
                    }
                IL_5DC:
                    if (!isLastChar && this.sent[j + 1].Equals('h'))
                    {
                        AkSoundEngine.SetSwitch("letters", "sh", this.soundSource);
                        j++;
                        goto IL_84B;
                    }
                    AkSoundEngine.SetSwitch("letters", "s", this.soundSource);
                    goto IL_84B;
                IL_756:
                    AkSoundEngine.SetSwitch("letters", "w", this.soundSource);
                    goto IL_84B;
                }
                yield return new WaitForSeconds((5f - this.npc.speed * 0.0385f) / 25f);
            }
            this.npc.isSpeaking = false;
            yield break;
        }

        private void Enunciate(char punc, int currentChar, int totalChars)
        {
            switch (punc)
            {
                case '!':
                    currentPitch = npc.pitch + 10f;
                    currentVolume = Mathf.Clamp(npc.volume + 30f, 0.0f, 100f);
                    break;
                case '?':
                    currentPitch = npc.pitch + (10 - Mathf.Clamp(totalChars - currentChar, 0, 10)) * 2;
                    currentVolume = npc.volume;
                    break;
                default:
                    currentPitch = npc.pitch;
                    currentVolume = npc.volume;
                    break;
            }
        }
    }
}*/