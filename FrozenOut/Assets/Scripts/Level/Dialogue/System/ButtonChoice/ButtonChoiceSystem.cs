using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue.System.Choice
{
    public class ButtonChoiceSystem : ChoiceSystem
    {
        public DialogueManager DialogueManager;

        public ButtonChoiceController ChoiceController;

        void Start()
        {
            ChoiceController.Close();
        }

        public override void StartChoice(IEnumerable<DialogueChoice> choices)
        {
            ChoiceController.Open();

            ChoiceController.SetButtons(choices);
        }

        public void SelectChoice(DialogueChoice choiceSelected)
        {
            DialogueManager.OnChoiceSelected(choiceSelected);

            ChoiceController.Close();

        }
    }
}