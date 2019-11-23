using System.Collections.Generic;
using Yarn.Unity;

using Assets.Scripts.Item;

namespace Assets.Scripts.Dialogue
{
    public class VariableStorageYarn : VariableStorageBehaviour
    {
        // Where we actually keep our variables
        private Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

        public Inventory inventory;

        void Start()
        {
            ResetToDefaults();
        }

        public override void ResetToDefaults ()
        {
            Clear ();

            // For each default variable that's been defined, parse the string
            // that the user typed in in Unity and store the variable
            foreach (ItemInfo item in inventory.inventoryItems) {

                SetValue (item.variableName, item.isInitiallyInInventory);

            }
        }

        public void SetValue(string variableName, bool value, bool includeLeading = true)
        {
            Yarn.Value yarnValue = new Yarn.Value(value);

            if(includeLeading)
            {
                variableName = AddLeading(variableName);
            }

            SetValue(variableName, yarnValue);
        }

        public string AddLeading(string variableName)
        {
            return "$" + variableName;
        }

        public override void SetValue(string variableName, Yarn.Value value)
        {
            // Copy this value into our list
            variables[variableName] = value;
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