﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue.Text;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class YarnInitialTextVariables : MonoBehaviour
    {
        public YarnManager DialogueManager;

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
                    DialogueManager.SetVariable<string>(key, variables[key]);
                }
            }
        }
    }
}