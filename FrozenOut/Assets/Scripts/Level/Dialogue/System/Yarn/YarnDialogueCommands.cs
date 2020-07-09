using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Yarn.Unity;

namespace Scripts.Level.Dialogue.Runner.YarnSpinner
{
    public class YarnDialogueCommands : MonoBehaviour
    {
        public YarnDialogueSystem YarnSystem;

        private DialogueRunner DialogueRunner => YarnSystem.DialogueRunner;

        private void Awake()
        {
            DialogueRunner.AddCommandHandler("startinstagram", StartInstagram);
        }

        [YarnCommand("giveitem")]
        public void PickItem(string itemVariableName, string quantity)
        {
            int realQuantity = int.Parse(quantity);

            YarnSystem.PickItem(itemVariableName, realQuantity);
        }

        [YarnCommand("useitem")]
        public void UseItem(string itemVariableName, string quantity)
        {
            int realQuantity = int.Parse(quantity);

            YarnSystem.UseItem(itemVariableName, realQuantity);
        }

        [YarnCommand("setanim")]
        public void SetAnimation(string npcName, string animation)
        {
            YarnSystem.SetNPCAnimation(npcName, animation);
        }

        [YarnCommand("setanimall")]
        public void SetAnimationAll(string npcName, string animation)
        {
            YarnSystem.SetNPCAnimationWithSimilarName(npcName, animation);
        }

        [YarnCommand("stopanim")]
        public void StopAnimation(string npcName)
        {
            YarnSystem.StopNPCAnimation(npcName);
        }

        [YarnCommand("stopanimall")]
        public void StopAnimationAll(string npcName)
        {
            YarnSystem.StopNPCAnimationWithSimilarName(npcName);
        }

        public void StartInstagram(string[] parameters, System.Action onComplete)
        {
            YarnSystem.DialogueManager.SwitchToInstagram(onComplete);
        }
    }
}