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

        private VariableStorageBehaviour VariableStorage => DialogueRunner.variableStorage;
        private DialogueUIBehaviour DialogueUI => DialogueRunner.dialogueUI;

        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        void Awake()
        {
            SetEvents();
        }

        void Start()
        {
            DialogueRunner.variableStorage = YarnVariableStorage.Instance;
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
            return DialogueRunner.isDialogueRunning;
        }

        public bool IsStarting()
        {
            return DialogueRunner.isDialogueStarting;
        }

        public bool IsReady()
        {
            return !IsRunning() && LevelManager.GetPlayerManager().IsGrounded;
        }

        public void StartDialogue(DialogueTalker talker)
        {
            talker.onStartTalk();
            DialogueRunner.StartDialogue(talker.TalkToNode);
        }

        public void StopDialogue()
        {
            DialogueRunner.Stop();
        }

        public void SetLanguage()
        {
            //TODO
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
        public event EventHandler<DialogueStartingEventArgs> Starting;
        public event EventHandler<DialogueEventArgs> Started;
        public event EventHandler Stopping, Stopped;
        public event EventHandler Completed, Ended;

        private void SetEvents()
        {
            DialogueRunner.Starting += (sender, args) => OnStarting(args);
            DialogueRunner.Started += (sender, args) => OnStarted(args);
            DialogueRunner.Stopping += (sender, args) => OnStopping();
            DialogueRunner.Stopped += (sender, args) => OnStopped();
            DialogueRunner.Completed += (sender, args) => OnCompleted();
            DialogueRunner.Ended += (sender, args) => OnEnded();
        }

        private void OnStarting(YarnDialogueStartingEventArgs yarnStartingArgs)
        {
            DialogueStartingEventArgs startingArgs = new DialogueStartingEventArgs(yarnStartingArgs.StartNode);
            startingArgs.Canceling += (sender, args) => yarnStartingArgs.Cancel = true;
            Starting?.Invoke(this, startingArgs);
        }

        protected void OnStarted(YarnDialogueEventArgs yarnStartedArgs)
        {
            DialogueEventArgs startedArgs = new DialogueEventArgs(yarnStartedArgs.StartNode);
            Started?.Invoke(this, startedArgs);
        }

        protected void OnStopping()
        {
            Stopping?.Invoke(this, EventArgs.Empty);
        }

        protected void OnStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        protected void OnCompleted()
        {
            Completed?.Invoke(this, EventArgs.Empty);
        }

        protected void OnEnded()
        {
            Ended?.Invoke(this, EventArgs.Empty);
        }
        #endregion

    }
}