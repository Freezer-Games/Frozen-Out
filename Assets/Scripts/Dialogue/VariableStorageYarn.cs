using System.Collections.Generic;
using Yarn.Unity;

namespace Assets.Scripts.Dialogue
{
    public class VariableStorageYarn : VariableStorageBehaviour
    {
        // Where we actually keeping our variables
        Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

        void Awake()
        {
            ResetToDefaults();
        }

        public override void ResetToDefaults()
        {
            Clear();
        }

        public override void SetValue(string variableName, Yarn.Value value)
        {
            // Copy this value into our list
            variables[variableName] = new Yarn.Value(value);
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