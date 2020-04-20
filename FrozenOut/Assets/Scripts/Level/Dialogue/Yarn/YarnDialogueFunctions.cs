using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.YarnSpinner
{
    public class YarnDialogueFunctions : MonoBehaviour
    {
        public YarnManager YarnManager;

        private DialogueRunner DialogueRunner => YarnManager.DialogueRunner;

        public void Load()
        {
            DialogueRunner.AddFunction("visited", 1, delegate (Yarn.Value[] parameters)
            {
                string nodeName = parameters[0].AsString;
                return YarnManager.GetBoolVariable("visited_" + nodeName);
            });
            DialogueRunner.onNodeComplete.AddListener(NodeComplete);
        }

        public void NodeComplete(string nodeName) {
            // Log that the node has been run
            YarnManager.SetVariable<bool>("visited_" + nodeName, true);
        }
    }
}