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

        public override void StartDialogue(DialogueActer acter)
        {
            DialogueRunner.StartDialogue(acter.TalkToNode);
        }

        public override void RequestNextLine()
        {
            DialogueController.RequestNextLine();
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

        public bool IsItemInInventory(string itemVariableName)
        {
            return DialogueManager.IsItemInInventory(itemVariableName);
        }
        public bool IsItemUsed(string itemVariableName)
        {
            return DialogueManager.IsItemUsed(itemVariableName);
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
        public void StopNPCAnimation(string npcName)
        {
            DialogueManager.StopNPCAnimation(npcName);
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
            DialogueController.DialogueStarted.AddListener(DialogueManager.OnDialogueStarted);
            DialogueController.DialogueEnded.AddListener(DialogueManager.OnDialogueEnded);
            DialogueController.LineStyleUpdated.AddListener(DialogueManager.OnLineStyleUpdated);
            DialogueController.LineNameUpdated.AddListener(DialogueManager.OnLineNameUpdated);
            DialogueController.LineDialogueUpdated.AddListener(DialogueManager.OnLineDialogueUpdated);
        }
        #endregion
    }
}