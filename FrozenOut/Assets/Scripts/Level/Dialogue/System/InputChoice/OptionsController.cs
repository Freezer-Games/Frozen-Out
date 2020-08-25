using System.Linq;
using UnityEngine.UI;

namespace Scripts.Level.Dialogue.System.Choice
{
    public class OptionsController : UIController
    {
        public InputChoiceSystem ChoiceSystem;

        public InputField TextField;

        private void Start()
        {
            TextField.onEndEdit.AddListener(OnTextInput);
        }

        public override void Open()
        {
            base.Open();

            TextField.enabled = true;
            TextField.text = "";
            TextField.Select();
        }

        public override void Close()
        {
            base.Close();

            TextField.enabled = false;
        }

        private void OnTextInput(string inputText)
        {
            DialogueChoice selectedOption = ChoiceSystem.DialogueChoices.Last();
            foreach (DialogueChoice option in ChoiceSystem.DialogueChoices)
            {
                if (option.Text.Contains(inputText))
                {
                    selectedOption = option;
                }
            }

            ChoiceSystem.SelectChoice(selectedOption);
        }
    }
}