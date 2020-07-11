using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts.Level.Dialogue.Runner.Conversation
{
    public class InputConversationSystem : ConversationSystem
    {
        public DialogueManager DialogueManager;

        public UIController OptionsController;

        public IEnumerable<DialogueOption> DialogueOptions;

        private bool Running;

        private void Start()
        {
            Running = false;
            CloseInput();
        }

        private void OpenInput()
        {
            OptionsController.Open();
        }

        private void CloseInput()
        {
            OptionsController.Close();
        }

        public override bool IsRunning()
        {
            return Running;
        }

        public override void StartOptions(IEnumerable<DialogueOption> options)
        {
            DialogueOptions = options;

            Running = true;
            OpenInput();
        }

        public void SelectOption(DialogueOption selectedOption)
        {
            DialogueManager.OnOptionSelected(selectedOption);

            Running = false;
            CloseInput();
        }
    }
}