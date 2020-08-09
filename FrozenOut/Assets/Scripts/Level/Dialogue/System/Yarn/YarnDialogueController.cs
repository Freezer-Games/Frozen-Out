using System.Collections;
using UnityEngine;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.System.YarnSpinner
{
    public class YarnDialogueController : DialogueUIBehaviour
    {
        public YarnDialogueSystem DialogueSystem;

        private readonly DialogueLineSeparator DialogueSeparator = new DialogueLineSeparator("++", ": ");

        private bool RequestedNextLine;
        private global::System.Action<int> OptionSelectionHandler;

        public void RequestNextLine()
        {
            RequestedNextLine = true;
        }

        public override void DialogueStart()
        {
            OnDialogueStart();
        }

        public override void DialogueComplete()
        {
            OnDialogueEnd();
        }

        public override Yarn.Dialogue.HandlerExecutionType RunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, global::System.Action onLineComplete)
        {
            StartCoroutine(DoRunLine(line, localisationProvider, onLineComplete));
            return Yarn.Dialogue.HandlerExecutionType.PauseExecution;
        }

        private IEnumerator DoRunLine(Yarn.Line line, ILineLocalisationProvider localisationProvider, global::System.Action onComplete)
        {
            OnLineStart();

            string text = localisationProvider.GetLocalisedTextForLine(line);

            // Sanity check
            if (text == null) {
                Debug.LogWarning($"Line {line.ID} doesn't have any localised text.");
                text = line.ID;
            }

            DialogueSeparator.Separate(text, out string characterStyleName, out string characterName, out string characterDialogue);

            OnLineStyleUpdated(characterStyleName);
            OnNameLineUpdate(characterName);
            OnDialogueLineUpdate(characterDialogue);

            while(!RequestedNextLine)
            {
                yield return null;
            }
            RequestedNextLine = false;

            yield return new WaitForEndOfFrame();

            OnLineEnd();

            onComplete();
        }

        public override void RunOptions(Yarn.OptionSet optionSet, ILineLocalisationProvider localisationProvider, global::System.Action<int> onOptionSelected)
        {
            DialogueSystem.OnChoicesStarted(optionSet.Options, localisationProvider);

            OptionSelectionHandler = onOptionSelected;
        }

        public override Yarn.Dialogue.HandlerExecutionType RunCommand(Yarn.Command command, global::System.Action onCommandComplete)
        {
            return Yarn.Dialogue.HandlerExecutionType.ContinueExecution;
        }

        public void SelectChoice(int optionID)
        {
            OptionSelectionHandler?.Invoke(optionID);
            OptionSelectionHandler = null;
        }

        #region Events
        public DialogueRunner.StringUnityEvent LineStyleUpdated;
        public DialogueRunner.StringUnityEvent LineNameUpdated;
        public DialogueRunner.StringUnityEvent LineDialogueUpdated;

        private void OnLineStyleUpdated(string styleName)
        {
            LineStyleUpdated?.Invoke(styleName);
        }

        private void OnNameLineUpdate(string name)
        {
            LineNameUpdated?.Invoke(name);
        }

        private void OnDialogueLineUpdate(string dialogue)
        {
            LineDialogueUpdated?.Invoke(dialogue);
        }
        #endregion
    }
}