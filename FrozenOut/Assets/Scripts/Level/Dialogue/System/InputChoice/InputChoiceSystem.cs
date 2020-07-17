using System.Collections.Generic;

namespace Scripts.Level.Dialogue.System.Choice
{
    public class InputChoiceSystem : ChoiceSystem
    {
        public DialogueManager DialogueManager;

        public UIController ChoiceController;

        public IEnumerable<DialogueChoice> DialogueChoices;

        private void Start()
        {
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

        public override void StartChoice(IEnumerable<DialogueChoice> choices)
        {
            DialogueChoices = choices;

            OpenInput();
        }

        public void SelectChoice(DialogueChoice selectedChoice)
        {
            DialogueManager.OnChoiceSelected(selectedChoice);

            CloseInput();
        }
    }
}