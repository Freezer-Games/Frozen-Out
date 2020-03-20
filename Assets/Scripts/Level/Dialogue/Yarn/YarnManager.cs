using System.Collections;
using System.Collections.Generic;

using Yarn.Unity;

namespace Scripts.Level.Dialogue
{
    public class YarnManager : MonoBehaviour, IDialogueManager
    {

        private DialogueRunner DialogueRunner;

        void Awake()
        {
            dialogueRunner = FindObjectOfType<DialogueRunner>();
        }

        public bool IsRunning()
        {
            return dialogueRunner.isDialogueRunning;
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
        
        public void SetValue<T>(string variableName, T value, bool includeLeading = true)
        {
            Yarn.Value yarnValue = new Yarn.Value(value);

            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            DialogueRunner.variableStorage.SetValue(variableName, yarnValue: yarnValue);
        }

        private Yarn.Value GetObjectValue(string variableName, bool includeLeading = true)
        {
            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            return DialogueRunner.variableStorage.GetValue(variableName);
        }

        private string AddLeading(string variableName)
        {
            return "$" + variableName;
        }
    }
}