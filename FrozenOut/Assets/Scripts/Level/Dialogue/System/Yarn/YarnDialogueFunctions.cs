using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.Runner.YarnSpinner
{
    public class YarnDialogueFunctions : MonoBehaviour
    {
        public YarnDialogueSystem YarnSystem;

        private DialogueRunner DialogueRunner => YarnSystem.DialogueRunner;

        public void Load()
        {
            DialogueRunner.AddFunction("visited", 1, delegate (Yarn.Value[] parameters)
            {
                string nodeName = parameters[0].AsString;
                return YarnSystem.GetBoolVariable("visited_" + nodeName);
            });
            DialogueRunner.onNodeComplete.AddListener(NodeComplete);

            DialogueRunner.AddFunction("has_item", 1, delegate (Yarn.Value[] parameters)
            {
                string itemVariableName = parameters[0].AsString;
                return YarnSystem.IsItemInInventory(itemVariableName);
            });

            DialogueRunner.AddFunction("used_item", 1, delegate (Yarn.Value[] parameters)
            {
                string itemVariableName = parameters[0].AsString;
                return YarnSystem.IsItemUsed(itemVariableName);
            });
        }

        public void NodeComplete(string nodeName) {
            // Log that the node has been run
            YarnSystem.SetVariable<bool>("visited_" + nodeName, true);
        }
    }
}