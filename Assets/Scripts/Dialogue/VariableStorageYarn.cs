using System.Collections.Generic;
using Yarn.Unity;

using Assets.Scripts.Item;
using System.Collections.ObjectModel;
using System;

namespace Assets.Scripts.Dialogue
{
    public class VariableStorageYarn : VariableStorageBehaviour
    {
        public Inventory inventory;

        // Where we actually keep our variables
        private readonly Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

        public ReadOnlyDictionary<string, Yarn.Value> Variables => new ReadOnlyDictionary<string, Yarn.Value>(variables);

        private GameManager gameManager;

        public bool IsInitialized { get; private set; }

        public event EventHandler<ReadOnlyDictionary<string, Yarn.Value>> Initialized;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            ResetToDefaults();
        }

        public override void ResetToDefaults ()
        {
            IsInitialized = false;
            Clear();

            SetValue(nameof(gameManager.TextSize), gameManager.TextSize);

            // For each default variable that's been defined, parse the string
            // that the user typed in in Unity and store the variable
            foreach (ItemInfo item in inventory.inventoryItems) {

                SetValue (item.variableName, item.isInitiallyInInventory);
            }

            OnInitialized(Variables);
        }

        protected virtual void OnInitialized(ReadOnlyDictionary<string, Yarn.Value> variables)
        {
            IsInitialized = true;
            Initialized?.Invoke(this, variables);
        }

        public void SetValue<T>(string variableName, T value, bool includeLeading = true)
        {
            Yarn.Value yarnValue = new Yarn.Value(value);

            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            SetValue(variableName, yarnValue: yarnValue);
        }

        private string AddLeading(string variableName)
        {
            return "$" + variableName;
        }

        public override void SetValue(string variableName, Yarn.Value yarnValue)
        {
            // Copy this value into our list
            variables[variableName] = yarnValue;
        }

        public override Yarn.Value GetValue(string variableName)
        {
            // If we don't have a variable with this name, return the null value
            if (variables.ContainsKey(variableName) == false)
                return Yarn.Value.NULL;

            return variables[variableName];
        }

        private Yarn.Value GetObjectValue(string variableName, bool includeLeading = true)
        {
            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            return GetValue(variableName);
        }

        public bool GetBoolValue(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsBool;
        }

        public string GetStringValue(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsString;
        }

        public float GetNumberValue(string variableName, bool includeLeading = true)
        {
            Yarn.Value yarnValue = GetObjectValue(variableName, includeLeading);
            return yarnValue.AsNumber;
        }

        public override void Clear()
        {
            variables.Clear();
        }
    }
}