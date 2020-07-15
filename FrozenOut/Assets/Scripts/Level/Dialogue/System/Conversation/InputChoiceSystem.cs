using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Level.Dialogue.Runner.Conversation
{
    public class InputChoiceSystem : ChoiceSystem
    {
        public DialogueManager DialogueManager;

        public UIController ChoiceController;

        public IEnumerable<DialogueChoice> DialogueChoices;

        private bool Running;

        private void Start()
        {
            Running = false;
            CloseInput();
        }

        private void OpenInput()
        {
            ChoiceController.Open();
        }

        private void CloseInput()
        {
            ChoiceController.Close();
        }

        public override bool IsRunning()
        {
            return Running;
        }

        public override void StartChoice(IEnumerable<DialogueChoice> choices)
        {
            DialogueChoices = choices;

            Running = true;
            OpenInput();
        }

        public void SelectChoice(DialogueChoice selectedChoice)
        {
            DialogueManager.OnChoiceSelected(selectedChoice);

            Running = false;
            CloseInput();
        }
    }
}