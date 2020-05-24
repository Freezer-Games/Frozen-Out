using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue.Utils;

namespace Scripts.Level.Dialogue.Runner.YarnSpinner
{
    public class YarnInitialTextVariables : MonoBehaviour
    {
        public YarnDialogueSystem YarnSystem;

        public TextAsset VariableFile;

        private FileVariableReader variableReader;

        void Awake()
        {
            variableReader = FileVariableReader.EqualFileVariableReader;
        }

        public void SetInitialVariables()
        {
            IDictionary<string, string> variables = variableReader.Extract(VariableFile.text);

            if (VariableFile != null)
            {
                foreach (string key in variables.Keys)
                {
                    YarnSystem.SetVariable<string>(key, variables[key]);
                }
            }
        }
    }
}