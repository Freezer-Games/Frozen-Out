using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.Runner.YarnSpinner
{
    public class YarnDialogueController : DialogueUIBehaviour
    {
        public YarnDialogueSystem DialogueSystem;

        private const string DefaultLineSeparator = ":";

        private bool RequestedNextLine;

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

        public override void RunOptions(Yarn.OptionSet optionSet, ILineLocalisationProvider localisationProvider, System.Action<int> onOptionSelected)
        {
            
        }

        public override Yarn.Dialogue.HandlerExecutionType RunCommand(Yarn.Command command, System.Action onCommandComplete)
        {
            return Yarn.Dialogue.HandlerExecutionType.ContinueExecution;
        }

        private void SeparateNameAndDialogue(string text, out string name, out string dialogue)
        {
            int indexOfNameSeparator = text.IndexOf(DefaultLineSeparator);
            name = text.Substring(0, indexOfNameSeparator);
            dialogue = text.Substring(indexOfNameSeparator + 2);
        }

        #region Events
        public DialogueRunner.StringUnityEvent LineNameUpdated;
        public DialogueRunner.StringUnityEvent LineDialogueUpdated;

        private void OnNameLineUpdate(string nameToDisplay)
        {
            LineNameUpdated?.Invoke(nameToDisplay);
        }

        private void OnDialogueLineUpdate(string dialogueToDisplay)
        {
            LineDialogueUpdated?.Invoke(dialogueToDisplay);
        }
        #endregion
    }
}