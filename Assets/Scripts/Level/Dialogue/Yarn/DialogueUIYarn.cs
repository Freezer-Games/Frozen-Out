using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using Yarn.Unity;

using Scripts.Level.Audio;

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
        private AudioManager AudioManager;
        private int currentLineNumber;

        void Start()
        {
            gameManager = GameManager.Instance;
            AudioManager = gameManager.CurrentLevelManager.GetAudioManager();
            DialogueManager = gameManager.CurrentLevelManager.GetDialogueManager();

            if (dialogueBoxGUI != null)
            {
                dialogueBoxGUI.SetActive(false);
            }

            ClearTexts();
        }

        void FixedUpdate()
        {
            if (dialogueManager.IsRunning() && Input.GetKey(gameManager.InputManager.NextDialogue))
            {
                localDelay /= localDelayMultiplier;
            }
        }

        public override IEnumerator RunLine(Yarn.Line line)
        {
            currentLineNumber++;

            string lineText = line.text;

			//Replace snippets with real text
            if (snippetSystems != null && snippetSystems.Length > 0)
            {
                foreach (var snippetSystem in snippetSystems)
                {
                    lineText = ParseSnippetSystem(lineText, snippetSystem);
                }
            }
			
			//Replace variables with real text
            if (variableSystem != null)
            {
                lineText = ParseSnippetSystem(lineText, variableSystem);
            }

            SeparateLine(lineText, out string characterName, out string characterDialogue);

            Tag selectedTextSizeTag = GetTextSizeTag(gameManager.TextSize);

            GetCurrentDialogueText(characterName);

            // Cambiar text size al nombre del personaje
            characterName = new DialogueTaggedText(selectedTextSizeTag, characterName).FullText;

			//Reset text, so only talking character name is shown
			mainNameText.text = "";
			otherNameText.text = "";
            currentNameText.text = characterName;

            currentDialogueText.gameObject.SetActive(true);

            if (letterDelay > 0.0f)
            {
                localDelay = letterDelay;

                IDialogueText completeCharacterDialogue = ComplexDialogueText.AnalyzeText(characterDialogue, RunLineLogger);

                completeCharacterDialogue = new DialogueTaggedText(selectedTextSizeTag, completeCharacterDialogue);

                foreach (string currentText in completeCharacterDialogue.Parse())
                {
                    currentDialogueText.text = currentText;
                    yield return new WaitForSeconds(localDelay);
                }
            }

            while (!Input.GetKeyDown(gameManager.InputManager.NextDialogue))
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            currentDialogueText.gameObject.SetActive(false);

        }

        private Tag GetTextSizeTag(double textSize)
        {
            TagOption selectedTextSizeStartTagOption
                                = new TagOption($"size={textSize}", TagFormat.RichTextTagFormat, TagOptionPosition.start);

            TagOption selectedTextSizeEndTagOption
                = new TagOption($"size", TagFormat.RichTextTagFormat, TagOptionPosition.end);

            Tag selectedTextSizeTag = new Tag(selectedTextSizeStartTagOption, selectedTextSizeEndTagOption);
            return selectedTextSizeTag;
        }

        private string ParseSnippetSystem<T>(string lineText, DialogueSnippetSystem<T> snippetSystem) where T : class
        {
            var snippets = snippetSystem.ParseSnippets(lineText, RunLineLogger);
            foreach (var snippet in snippets)
            {
                lineText = lineText.Replace(snippet.FullName, snippet.Value.ToString());
            }

            return lineText;
        }

        private void RunLineLogger(ParsingException parsingException)
        {
            Debug.LogError($"Error: {parsingException.GetFullMessage(currentLineNumber)}");
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