using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Yarn.Unity;

using Scripts;
using Scripts.Inventory;

namespace Scripts.Level.Dialogue
{
    public class VariableStorageYarn : VariableStorageBehaviour
    {

        public ReadOnlyDictionary<string, Yarn.Value> Variables => new ReadOnlyDictionary<string, Yarn.Value>(variables);
        private readonly Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

        public bool IsInitialized { get; private set; }

        public event EventHandler<ReadOnlyDictionary<string, Yarn.Value>> Initialized;

        private GameManager gameManager;

        void Start()
        {
            gameManager = GameManager.Instance;
            ResetToDefaults();
        }

        public override void ResetToDefaults ()
        {
            IsInitialized = false;
            Clear();

            SetTextSizeValue();
            SetInventoryValues();

            OnInitialized(Variables);
        }

        private void SetTextSizeValue()
        {
            SetValue(gameManager.getTextSizeName(), gameManager.getTextSize());
        }

        private void SetInventoryValues()
        {
            Inventory inventory = gameManager.currentLevelManager.getInventory();
            foreach (ItemInfo item in inventory.inventoryItems) {

                SetValue(item.variableName, item.isInitiallyInInventory);
            }
        }

        protected virtual void OnInitialized(ReadOnlyDictionary<string, Yarn.Value> variables)
        {
            IsInitialized = true;
            Initialized?.Invoke(this, variables);
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

        public override void Clear()
        {
            variables.Clear();
        }
    }
}