using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class YarnVariableStorage : VariableStorageBehaviour
    {

        #region Singleton
        public static YarnVariableStorage Instance
        {
            get
            {
                return Singleton;
            }
        }
        private static YarnVariableStorage Singleton;
        private void CreateSingleton()
        {
            if (Singleton != null && Singleton != this)
            {
                Destroy(this.gameObject);
            } else {
                Singleton = this;
            }
        }
        #endregion

        public ReadOnlyDictionary<string, Yarn.Value> Variables => new ReadOnlyDictionary<string, Yarn.Value>(variables);
        private readonly Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value>();

        public bool IsInitialized {
            get;
            private set;
        }

        void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            ResetToDefaults();
        }

        public override void ResetToDefaults ()
        {
            IsInitialized = false;
            Clear();
            // Reset to defaults
            IsInitialized = true;
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