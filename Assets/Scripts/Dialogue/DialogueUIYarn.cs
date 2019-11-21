﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using Yarn.Unity;
using Assets.Scripts.Dialogue.Texts;

namespace Assets.Scripts.Dialogue
{
    public class DialogueUIYarn : DialogueUIBehaviour
    {
        public const string MAIN_NAME = "Pol";
        public const string LINE_SEPARATOR = ": ";

        //Where name of character will be displayed
        public Text mainNameText;
        //Where current dialogue will be displayed
        public Text mainDialogueText;
        public Text otherNameText;
        public Text otherDialogueText;

        //Place where name and dialogue will be contained
        public GameObject dialogueBoxGUI;
        public Text continuePrompt;

        public float letterDelay = 0.1f;

        public AudioClip audioClip;

        private Text currentNameText, currentDialogueText;

        private AudioSource audioSource;
        private float localDelay;
        private readonly float localDelayMultiplier = 1.5f;

        private DialogueRunner dialogueSystem;
        private int currentLineNumber;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            dialogueSystem = FindObjectOfType<DialogueRunner>();

            if (dialogueBoxGUI != null)
            {
                dialogueBoxGUI.SetActive(false);
            }
            if (continuePrompt != null)
            {
                continuePrompt.gameObject.SetActive(false);
            }

            mainNameText.text = "";
            mainDialogueText.text = "";

            otherNameText.text = "";
            otherDialogueText.text = "";
        }

        void FixedUpdate()
        {
            if (dialogueSystem.isDialogueRunning && Input.anyKey)
            {
                localDelay /= localDelayMultiplier;
            }
        }

        public override IEnumerator RunLine(Yarn.Line line)
        {
            currentLineNumber++;

            SeparateLine(line.text, out string characterName, out string characterDialogue);

            GetCurrentDialogueText(characterName);

            currentDialogueText.gameObject.SetActive(true);

            if (letterDelay > 0.0f)
            {
                localDelay = letterDelay;

                IDialogueText completeCharacterDialogue = ComplexDialogueText.AnalyzeText(characterDialogue, 
                    parsingException => Debug.LogError($"Error: {parsingException.GetFullMessage(currentLineNumber)}"));

                foreach (string currentText in completeCharacterDialogue.Parse())
                {
                    currentDialogueText.text = currentText;
                    yield return new WaitForSeconds(localDelay);
                }
            }
            currentDialogueText.text = characterDialogue;

            // Show the 'press any key' prompt when done, if we have one
            if (continuePrompt != null)
            {
                continuePrompt.gameObject.SetActive(true);
            }

            while (!Input.anyKeyDown)
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            currentDialogueText.gameObject.SetActive(false);

            if (continuePrompt != null)
            {
                continuePrompt.gameObject.SetActive(false);
            }
        }

        private void GetCurrentDialogueText(string characterName)
        {
            currentNameText = mainNameText;
            currentDialogueText = mainDialogueText;
            if (characterName != MAIN_NAME)
            {
                currentNameText = otherNameText;
                currentDialogueText = otherDialogueText;
            }

            currentNameText.text = characterName;
        }

        public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser)
        {
            yield return null;
        }

        public override IEnumerator RunCommand(Yarn.Command command)
        {
            yield return null;
        }

        public override IEnumerator DialogueStarted()
        {
            // Enable the dialogue controls.
            if (dialogueBoxGUI != null)
            {
                dialogueBoxGUI.SetActive(true);
            }

            mainNameText.text = "";
            otherNameText.text = "";

            currentLineNumber = 0;

            yield break;
        }

        public override IEnumerator DialogueComplete()
        {
            // Hide the dialogue interface.
            if (dialogueBoxGUI != null)
                dialogueBoxGUI.SetActive(false);

            yield break;
        }

        public void SeparateLine(string text, out string name, out string dialogue)
        {
            int indexOfNameSeparator = text.IndexOf(LINE_SEPARATOR);
            name = text.Substring(0, indexOfNameSeparator);
            dialogue = text.Substring(indexOfNameSeparator + 2);
        }
    }

    public enum TagOptionPosition { start, end }
}