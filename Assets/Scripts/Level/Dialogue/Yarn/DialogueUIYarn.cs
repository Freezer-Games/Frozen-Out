using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using Yarn.Unity;

using Scripts.Level.Sound;

namespace Scripts.Level.Dialogue
{
    public class DialogueUIYarn : DialogueUIBehaviour
    {
        private const string MAIN_NAME = "Pol";
        private const string LINE_SEPARATOR = ": ";

        //Where name of character will be displayed
        public Text mainNameText;
        //Where current dialogue will be displayed
        public Text mainDialogueText;
        public Text otherNameText;
        public Text otherDialogueText;

        //Place where name and dialogue will be contained
        public GameObject dialogueBoxGUI;

        public float letterDelay = 0.1f;

        private Text currentNameText;
        private Text currentDialogueText;

        private float localDelay;
        private readonly float localDelayMultiplier = 1.5f;

        private GameManager gameManager;
        private IDialogueManager DialogueManager;
        private SoundManager SoundManager;
        private int currentLineNumber;

        void Start()
        {
            gameManager = GameManager.Instance;
            DialogueManager = gameManager.CurrentLevelManager.GetDialogueManager();
            SoundManager = gameManager.CurrentLevelManager.GetSoundManager();

            if (dialogueBoxGUI != null)
            {
                dialogueBoxGUI.SetActive(false);
            }

            ClearTexts();
        }

        void FixedUpdate()
        {
            if (DialogueManager.IsRunning() && Input.GetKey(gameManager.SettingsManager.NextDialogueKey))
            {
                localDelay /= localDelayMultiplier;
            }
        }

        public override IEnumerator RunLine(Yarn.Line line)
        {
            currentLineNumber++;

            string lineText = line.text;

            SeparateLine(lineText, out string characterName, out string characterDialogue);

            GetCurrentDialogueText(characterName);

			//Reset text, so only talking character name is shown
			mainNameText.text = "";
			otherNameText.text = "";
            currentNameText.text = characterName;

            currentDialogueText.gameObject.SetActive(true);

            if (letterDelay > 0.0f)
            {
                localDelay = letterDelay;

                /*foreach (string currentText in completeCharacterDialogue.Parse())
                {
                    currentDialogueText.text = currentText;
                    yield return new WaitForSeconds(localDelay);
                }*/
            }

            while (!Input.GetKeyDown(gameManager.SettingsManager.NextDialogueKey))
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            currentDialogueText.gameObject.SetActive(false);

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

        private void ClearTexts()
        {
            mainNameText.text = "";
            mainDialogueText.text = "";

            otherNameText.text = "";
            otherDialogueText.text = "";
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