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

        private HashSet<string> _visitedNodes = new HashSet<string>();

        public void Load()
        {
            DialogueRunner.AddFunction("visited", 1, delegate (Yarn.Value[] parameters)
            {
                var nodeName = parameters[0];
                return _visitedNodes.Contains(nodeName.AsString);
            });
            DialogueRunner.onNodeComplete.AddListener(NodeComplete);
        }

        public void NodeComplete(string nodeName) {
            // Log that the node has been run.
            _visitedNodes.Add(nodeName);
        }
    }
}