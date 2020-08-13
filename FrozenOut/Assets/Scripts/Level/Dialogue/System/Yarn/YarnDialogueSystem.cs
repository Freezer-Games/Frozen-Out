using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.System.YarnSpinner
{
    public class YarnDialogueSystem : MainDialogueSystem
    {
        public DialogueManager DialogueManager;

        public DialogueRunner DialogueRunner;
        public YarnDialogueFunctions DialogueFunctions;
        public YarnInitialTextVariables InitialTextVariables;
        public YarnDialogueController DialogueController;

        private VariableStorageBehaviour VariableStorage => DialogueRunner.variableStorage;
        
        void Start()
        {
            ConvertEvents();
            DialogueFunctions.Load();

            SetInitialVariables();
        }

        public override bool IsRunning()
        {
            return DialogueRunner.IsDialogueRunning;
        }

        public override void StartDialogue(DialogueActer acter)
        {
            DialogueRunner.StartDialogue(acter.StoryNode);
        }

        public override void RequestNextLine()
        {
            DialogueController.RequestNextLine();
        }

        public override void RequestSelectChoice(DialogueChoice choice)
        {
            DialogueController.SelectChoice(choice.ID);
        }

        public override void Stop()
        {
            DialogueRunner.Stop();

            DialogueManager.OnDialogueEnded();
        }

        public override void SetLanguage(Locale locale)
        {
            DialogueRunner.textLanguage = locale.Identifier.Code;
        }

        public override void Switch()
        {
            onSwitchBack?.Invoke();
            onSwitchBack = null;
        }

        private global::System.Action onSwitchBack;
        public void SwitchToSecondary(string systemName, global::System.Action onComplete)
        {
            onSwitchBack = onComplete;

            DialogueManager.SwitchToSecondary(systemName);
        }

        #region Proxy
        public bool IsItemInInventory(string itemVariableName)
        {
            return DialogueManager.IsItemInInventory(itemVariableName);
        }
        public bool IsItemUsed(string itemVariableName)
        {
            return DialogueManager.IsItemUsed(itemVariableName);
        }
        public int QuantityOfItem(string itemVariableName)
        {
            return DialogueManager.QuantityOfItem(itemVariableName);
        }
        public bool MarkMissionDone(string missionVariableName)
        {
            return DialogueManager.MarkMissionDone(missionVariableName);
        }
        public void PickItem(string itemVariableName, int quantity)
        {
            DialogueManager.PickItem(itemVariableName, quantity);
        }
        public void UseItem(string itemVariableName, int quantity)
        {
            DialogueManager.UseItem(itemVariableName, quantity);
        }
        public void SetNPCAnimation(string npcName, string animation)
        {
            DialogueManager.SetNPCAnimation(npcName, animation);
        }
        public void SetNPCAnimationWithSimilarName(string npcName, string animation)
        {
            DialogueManager.SetNPCAnimationWithSimilarName(npcName, animation);
        }
        public void StopNPCAnimation(string npcName)
        {
            DialogueManager.StopNPCAnimation(npcName);
        }
        public void StopNPCAnimationWithSimilarName(string npcName)
        {
            DialogueManager.StopNPCAnimationWithSimilarName(npcName);
        }
        #endregion

        public override bool GetBoolVariable(string variableName)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName);
            return yarnValue.AsBool;
        }

        public override string GetStringVariable(string variableName)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName);
            return yarnValue.AsString;
        }

        public override float GetNumberVariable(string variableName)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName);
            return yarnValue.AsNumber;
        }
        
        public override void SetVariable<T>(string variableName, T value)
        {
            variableName = AddLeading(variableName);

            Yarn.Value yarnValue = new Yarn.Value(value);

            VariableStorage.SetValue(variableName, yarnValue);
        }

        private Yarn.Value GetObjectValue(string variableName)
        {
            variableName = AddLeading(variableName);

            return VariableStorage.GetValue(variableName);
        }

        private string AddLeading(string variableName)
        {
            return "$" + variableName;
        }

        private void SetInitialVariables()
        {
            SetVariable<float>("textSize", DialogueManager.GetTextSize());

            InitialTextVariables.SetInitialVariables();
        }

        #region Events
        private void ConvertEvents()
        {
            DialogueController.DialogueStarted.AddListener(DialogueManager.OnDialogueStarted);
            DialogueController.DialogueEnded.AddListener(DialogueManager.OnDialogueEnded);

            DialogueController.LineStarted.AddListener(DialogueManager.OnLineStarted);
            DialogueController.LineStyleUpdated.AddListener(DialogueManager.OnLineStyleUpdated);
            DialogueController.LineNameUpdated.AddListener(DialogueManager.OnLineNameUpdated);
            DialogueController.LineDialogueUpdated.AddListener(DialogueManager.OnLineDialogueUpdated);
        }

        public void OnChoicesStarted(Yarn.OptionSet.Option[] options, ILineLocalisationProvider localisationProvider)
        {
            IEnumerable<DialogueChoice> dialogueChoices = options.Select((option) => {

                string text = localisationProvider.GetLocalisedTextForLine(option.Line);

                // Sanity check
                if (text == null)
                {
                    Debug.LogWarning($"Line {option.Line.ID} doesn't have any localised text.");
                    text = option.Line.ID;
                }

                DialogueChoice dialogueChoice = new DialogueChoice()
                {
                    Text = text,
                    ID = option.ID
                };

                return dialogueChoice;
            });

            DialogueManager.OnChoicesStarted(dialogueChoices);
        }
        #endregion
    }
}