using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue.Utils;

namespace Scripts.Level.Dialogue.System.YarnSpinner
{
    public class YarnInitialTextVariables : MonoBehaviour
    {
        public YarnDialogueSystem YarnSystem;

        public TextAsset VariableFile;

        private FileVariableReader VariableReader;

        void Awake()
        {
            VariableReader = FileVariableReader.EqualFileVariableReader;
        }

        public void SetInitialVariables()
        {
            IDictionary<string, string> variables = VariableReader.Extract(VariableFile.text);

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