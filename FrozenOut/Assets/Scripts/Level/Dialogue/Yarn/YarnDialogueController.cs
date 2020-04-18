using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using Yarn.Unity;

using Scripts.Level.Dialogue.Text;
using Scripts.Level.Dialogue.Text.Tag;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    [RequireComponent(typeof(Canvas))]
    public class YarnDialogueController : DialogueUIBehaviour
    {
        public YarnManager DialogueManager;

        public Canvas DialogueCanvas;
        private bool IsOpen => DialogueCanvas.enabled;

        private const string LINE_SEPARATOR = ":";

        private float LetterDelay = 0.1f;
        private float NextDialogueDelay = 0.3f;
        private bool UserRequestedAllLine = false;
        private bool UserRequestedNextLine = false;

        void Awake()
        {
            Close();
        }

        void Update()
        {
            if (DialogueManager.IsRunning() && !UserRequestedAllLine && Input.GetKeyDown(DialogueManager.GetNextDialogueKey()))
            {
                UserRequestedAllLine = true;
            }
            if (DialogueManager.IsRunning() && !UserRequestedNextLine && Input.GetKey(DialogueManager.GetNextDialogueKey()))
            {
                UserRequestedNextLine = true;
            }
        }

        public void Open()
        {
            DialogueCanvas.enabled = true;
        }

        public void Close()
        {
            DialogueCanvas.enabled = false;
        }

        public override void DialogueStart()
        {
            Open();

            OnDialogueStart();
        }

        public override void DialogueComplete()
        {
            OnDialogueEnd();

            Close();
        }

        public override Yarn.Dialogue.HandlerExecutionType RunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, System.Action onLineComplete)
        {
            StartCoroutine(DoRunLine(line, localisationProvider, onLineComplete));
            return Yarn.Dialogue.HandlerExecutionType.PauseExecution;
        }

        private IEnumerator DoRunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, System.Action onComplete)
        {
            OnLineStart();

            string text = localisationProvider.GetLocalisedTextForLine(line);

            // Sanity check
            if (text == null) {
                Debug.LogWarning($"Line {line.ID} doesn't have any localised text.");
                text = line.ID;
            }

            SeparateNameAndDialogue(text, out string characterName, out string characterDialogue);

            OnNameLineUpdate(characterName);

            if (LetterDelay > 0.0f)
            {
                // Antes de hacer nada se analiza el texto y se clasifican internamente las partes con tags y las simples
                IDialogueText completeCharacterDialogue = ComplexDialogueText.AnalyzeText(characterDialogue);

                UserRequestedAllLine = false;

                foreach (string currentText in completeCharacterDialogue.Parse())
                {
                    OnDialogueLineUpdate(currentText);

                    if (UserRequestedAllLine)
                    {
                        OnDialogueLineUpdate(characterDialogue);
                        break;
                    }

                    yield return new WaitForSeconds(LetterDelay);
                }
            }
            else
            {
                OnDialogueLineUpdate(characterDialogue);
            }

            OnLineFinishDisplaying();

            yield return new WaitForSeconds(NextDialogueDelay);

            UserRequestedNextLine = false;

            while(!UserRequestedNextLine)
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            OnLineEnd();

            onComplete();
        }

        public override void RunOptions(Yarn.OptionSet optionSet, ILineLocalisationProvider localisationProvider, System.Action<int> onOptionSelected)
        {
            
        }

        public override Yarn.Dialogue.HandlerExecutionType RunCommand(Yarn.Command command, System.Action onCommandComplete)
        {
            return Yarn.Dialogue.HandlerExecutionType.ContinueExecution;
        }

        private void SeparateNameAndDialogue(string text, out string name, out string dialogue)
        {
            int indexOfNameSeparator = text.IndexOf(LINE_SEPARATOR);
            name = text.Substring(0, indexOfNameSeparator);
            dialogue = text.Substring(indexOfNameSeparator + 2);
        }

        #region Events
        public DialogueRunner.StringUnityEvent LineNameUpdated;
        public DialogueRunner.StringUnityEvent LineDialogueUpdated;

        private void OnDialogueStart()
        {
            DialogueStarted?.Invoke();
        }

        private void OnDialogueEnd()
        {
            DialogueEnded?.Invoke();
        }

        private void OnLineStart()
        {
            LineStarted?.Invoke();
        }

        private void OnNameLineUpdate(string nameToDisplay)
        {
            LineNameUpdated?.Invoke(nameToDisplay);
        }

        private void OnDialogueLineUpdate(string dialogueToDisplay)
        {
            LineDialogueUpdated?.Invoke(dialogueToDisplay);
        }

        private void OnLineEnd()
        {
            LineEnded?.Invoke();
        }

        private void OnLineFinishDisplaying()
        {
            LineFinishDisplaying?.Invoke();
        }
        #endregion
    }
}