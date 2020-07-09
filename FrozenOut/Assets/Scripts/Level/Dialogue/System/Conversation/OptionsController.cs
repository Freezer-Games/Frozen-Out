using System.Linq;
using UnityEngine.UI;

namespace Scripts.Level.Dialogue.Runner.Conversation
{
    public class OptionsController : UIController
    {
        public InputConversationSystem ConversationSystem;

        public InputField TextField;

        private void Start()
        {
            TextField.onEndEdit.AddListener(OnTextInput);
        }

        public override void Open()
        {
            base.Open();

            // Sanity check
            TextField.text = "";
        }

        private void OnTextInput(string inputText)
        {
            DialogueOption selectedOption = ConversationSystem.DialogueOptions.Last();
            foreach (DialogueOption option in ConversationSystem.DialogueOptions)
            {
                if (option.Text.Contains(inputText))
                {
                    selectedOption = option;
                }
            }

            ConversationSystem.SelectOption(selectedOption);
        }
    }
}