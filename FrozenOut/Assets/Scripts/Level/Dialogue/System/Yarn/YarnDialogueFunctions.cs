using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.System.YarnSpinner
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

            DialogueRunner.AddFunction("quantity_item", 1, delegate (Yarn.Value[] parameters)
            {
                string itemVariableName = parameters[0].AsString;
                return YarnSystem.QuantityOfItem(itemVariableName);
            });

            DialogueRunner.AddFunction("random_bool", 0, delegate (Yarn.Value[] parameters)
            {
                float random = Random.value;
                return random > 0.5f;
            });
        }

        public void NodeComplete(string nodeName) {
            // Log that the node has been run
            YarnSystem.SetVariable<bool>("visited_" + nodeName, true);
        }
    }
}