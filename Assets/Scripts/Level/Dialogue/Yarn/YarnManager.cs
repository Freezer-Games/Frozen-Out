using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Yarn.Unity;

using Scripts.Settings;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class YarnManager : MonoBehaviour, IDialogueManager
    {
        
        public LevelManager LevelManager;

        public DialogueRunner DialogueRunner;
        public YarnDialoguePromptController DialoguePromptController;
        public YarnDialogueFunctions DialogueFunctions;

        private VariableStorageBehaviour VariableStorage => DialogueRunner.variableStorage;
        private DialogueUIBehaviour DialogueController => DialogueRunner.dialogueUI;

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
        }

        public KeyCode GetNextDialogueKey()
        {
            return SettingsManager.NextDialogueKey;
        }

        public KeyCode GetInteractKey()
        {
            return SettingsManager.InteractKey;
        }

        public void OpenTalkPrompt(DialogueTalker dialogueTalker)
        {
            DialoguePromptController.Open(dialogueTalker);
        }

        public void CloseTalkPrompt()
        {
            DialoguePromptController.Close();
        }

        public bool IsRunning()
        {
            return DialogueRunner.IsDialogueRunning;
        }

        public bool IsReady()
        {
            return !IsRunning() && LevelManager.GetPlayerManager().IsGrounded;
        }

        public void StartDialogue(DialogueTalker talker)
        {
            CurrentTalker = talker;
            DialogueRunner.StartDialogue(talker.TalkToNode);
        }

        public void StopDialogue()
        {
            DialogueRunner.Stop();
        }

        public void SetLanguage()
        {
            DialogueRunner.textLanguage = SettingsManager.Locale.Identifier.Code;
            Debug.Log(SettingsManager.Locale.Identifier.Code);
        }

        public bool GetBoolVariable(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsBool;
        }

        public string GetStringVariable(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsString;
        }

        public float GetNumberVariable(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsNumber;
        }
        
        public void SetVariable<T>(string variableName, T value, bool includeLeading = true)
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

        #region Events
        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Completed, Ended;

        private void SetEvents()
        {
            DialogueController.DialogueStarted.AddListener(OnStarted);
            DialogueController.DialogueEnded.AddListener(OnEnded);

            Started += OnStartDialogue;
            Ended += OnEndDialogue;
        }

        private void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        private void OnStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        private void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        private void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
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