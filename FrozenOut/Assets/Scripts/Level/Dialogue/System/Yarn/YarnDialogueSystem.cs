using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Yarn.Unity;

using UnityEngine.Localization;

namespace Scripts.Level.Dialogue.Runner.YarnSpinner
{
    public class YarnDialogueSystem : DialogueSystem
    {
        public DialogueManager DialogueManager;

        public DialogueRunner DialogueRunner;
        public YarnDialogueFunctions DialogueFunctions;
        public YarnInitialTextVariables InitialTextVariables;
        public YarnDialogueController DialogueController;

        private VariableStorageBehaviour VariableStorage => DialogueRunner.variableStorage;

        void Awake()
        {
            DialogueRunner.variableStorage = YarnVariableStorage.Instance;
        }
        
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

        public override void StartDialogue(DialogueTalker talker)
        {
            DialogueRunner.StartDialogue(talker.TalkToNode);
        }

        public override void RequestNextLine()
        {
            DialogueController.RequestNextLine();
        }

        public override void Stop()
        {
            DialogueRunner.Stop();
        }

        public override void SetLanguage(Locale locale)
        {
            DialogueRunner.textLanguage = locale.Identifier.Code;
        }

        public override bool GetBoolVariable(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsBool;
        }

        public override string GetStringVariable(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsString;
        }

        public override float GetNumberVariable(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsNumber;
        }
        
        public override void SetVariable<T>(string variableName, T value, bool includeLeading = true)
        {
            Yarn.Value yarnValue = new Yarn.Value(value);

            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            VariableStorage.SetValue(variableName, yarnValue);
        }

        private Yarn.Value GetObjectValue(string variableName, bool includeLeading = true)
        {
            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

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
            DialogueController.DialogueStarted.AddListener(DialogueManager.OnStarted);
            DialogueController.DialogueEnded.AddListener(DialogueManager.OnEnded);
            DialogueController.LineNameUpdated.AddListener(DialogueManager.OnLineNameUpdated);
            DialogueController.LineDialogueUpdated.AddListener(DialogueManager.OnLineDialogueUpdated);
        }
        #endregion
    }
}