using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Level.Dialogue.System.Ink
{
    public class InkCommands : MonoBehaviour
    {
        public InkDialogueSystem InkSystem;

        public void ChooseFunctionFromTags(IEnumerable<string> tags)
        {
            if(tags.Count() > 0)
            {
                foreach(string tag in tags)
                {
                    string[] words = tag.Split(' ');

                    string functionName = words.ElementAt(0);
                    IEnumerable<string> parameters = words.Skip(1);

                    ChooseFunction(functionName, parameters);
                }
            }
        }

        public void ChooseFunction(string function, IEnumerable<string> parameters)
        {
            switch(function)
            {
                case "giveitem":
                    PickItem(parameters.ElementAt(0), parameters.ElementAt(1));
                    break;
                case "useitem":
                    UseItem(parameters.ElementAt(0), parameters.ElementAt(1));
                    break;
                case "setanim":
                    SetAnimation(parameters.ElementAt(0), parameters.ElementAt(1));
                    break;
                case "setanimall":
                    SetAnimationAll(parameters.ElementAt(0), parameters.ElementAt(1));
                    break;
                case "stopanim":
                    StopAnimation(parameters.ElementAt(0));
                    break;
                case "stopanimall":
                    StopAnimationAll(parameters.ElementAt(0));
                    break;
            }
        }

        private void PickItem(string itemVariableName, string quantity)
        {
            int realQuantity = int.Parse(quantity);

            InkSystem.PickItem(itemVariableName, realQuantity);
        }

        private void UseItem(string itemVariableName, string quantity)
        {
            int realQuantity = int.Parse(quantity);

            InkSystem.UseItem(itemVariableName, realQuantity);
        }

        private void SetAnimation(string npcName, string animation)
        {
            InkSystem.SetNPCAnimation(npcName, animation);
        }

        private void SetAnimationAll(string npcName, string animation)
        {
            InkSystem.SetNPCAnimationWithSimilarName(npcName, animation);
        }

        private void StopAnimation(string npcName)
        {
            InkSystem.StopNPCAnimation(npcName);
        }

        private void StopAnimationAll(string npcName)
        {
            InkSystem.StopNPCAnimationWithSimilarName(npcName);
        }
    }
}