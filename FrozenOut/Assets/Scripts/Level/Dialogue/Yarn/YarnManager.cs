using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Yarn.Unity;

using Scripts.Settings;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class YarnManager : DialogueManager
    {
        public LevelManager LevelManager;

        public DialogueRunner DialogueRunner;
        public YarnDialoguePromptController DialoguePromptController;
        public YarnDialogueFunctions DialogueFunctions;
        public YarnInitialTextVariables InitialTextVariables;
        public YarnDialogueController DialogueController;

        private VariableStorageBehaviour VariableStorage => DialogueRunner.variableStorage;

        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        private DialogueTalker CurrentTalker;

        void Awake()
        {
            DialogueRunner.variableStorage = YarnVariableStorage.Instance;
            SetLanguage();
        }
        
        void Start()
        {
            SetEvents();
            DialogueFunctions.Load();

            SetInitialVariables();
        }

        public KeyCode GetNextDialogueKey()
        {
            return SettingsManager.NextDialogueKey;
        }

        public KeyCode GetInteractKey()
        {
            return SettingsManager.InteractKey;
        }

        public float GetTextSize()
        {
            return LevelManager.GetSettingsManager().TextSize;
        }

        public override void OpenTalkPrompt(DialogueTalker dialogueTalker)
        {
            DialoguePromptController.Open(dialogueTalker);
        }

        public override void CloseTalkPrompt()
        {
            DialoguePromptController.Close();
        }

        public override bool IsRunning()
        {
            return DialogueRunner.IsDialogueRunning;
        }

        public override bool IsReady()
        {
            return !IsRunning() && LevelManager.GetPlayerManager().IsGrounded;
        }

        public override void StartDialogue(DialogueTalker talker)
        {
            CurrentTalker = talker;
            DialogueController.AddStyle(talker.Name, talker.Style);
            DialogueRunner.StartDialogue(talker.TalkToNode);
        }

        public override void StopDialogue()
        {
            DialogueRunner.Stop();
        }

        public override void SetLanguage()
        {
            DialogueRunner.textLanguage = SettingsManager.Locale.Identifier.Code;
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
            SetVariable<float>("textSize", GetTextSize());

            InitialTextVariables.SetInitialVariables();
        }

        #region Events
        private void SetEvents()
        {
            DialogueController.DialogueStarted.AddListener(OnStarted);
            DialogueController.DialogueEnded.AddListener(OnEnded);

            Started += OnStartDialogue;
            Ended += OnEndDialogue;
        }

        private void OnStartDialogue(object sender, EventArgs args)
        {
            CurrentTalker?.OnStartTalk();
        }

        private void OnEndDialogue(object sender, EventArgs args)
        {
            CurrentTalker?.OnEndTalk();
            CurrentTalker = null;
        }
        #endregion
    }
}