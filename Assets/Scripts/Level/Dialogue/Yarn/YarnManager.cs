using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Yarn.Unity;

using Scripts.Settings;

namespace Scripts.Level.Dialogue
{
    public class YarnManager : MonoBehaviour, IDialogueManager
    {
        
        public LevelManager LevelManager;

        public DialogueRunner DialogueRunner;

        private VariableStorageBehaviour VariableStorage => DialogueRunner.variableStorage;
        private DialogueUIBehaviour DialogueUI => DialogueRunner.dialogueUI;

        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        void Awake()
        {
            DialogueRunner = FindObjectOfType<DialogueRunner>();
            DialogueRunner.variableStorage = VariableStorageYarn.Instance;

            DialogueRunner.Starting += (sender, args) => OnStarting();
            DialogueRunner.Started += (sender, args) => OnStarted();
            DialogueRunner.Stopping += (sender, args) => OnStopping();
            DialogueRunner.Stopped += (sender, args) => OnStopped();
            DialogueRunner.Completed += (sender, args) => OnCompleted();
            DialogueRunner.Ended += (sender, args) => OnEnded();
        }

        public KeyCode GetNextDialogueKey()
        {
            return SettingsManager.NextDialogueKey;
        }

        public bool IsRunning()
        {
            return DialogueRunner.isDialogueRunning;
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
        public event EventHandler Starting, Started;
        public event EventHandler Stopping, Stopped;
        public event EventHandler Completed, Ended;

        protected void OnStarting()
        {
            Starting?.Invoke(this, EventArgs.Empty);
        }

        protected void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
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