using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Scripts.Level.Dialogue.System.Choice
{
    public class ButtonChoice : MonoBehaviour
    {
        public Button Button;
        public UnityEngine.UI.Text ButtonText;

        private ButtonChoiceController Controller;
        private DialogueChoice Choice;

        public void SetButton(DialogueChoice choice, ButtonChoiceController controller)
        {
            this.Choice = choice;
            this.Controller = controller;

            ButtonText.text = choice.Text;
            Button.onClick.AddListener(ButtonClick);
        }

        public void ButtonClick()
        {
            Controller.SelectChoice(Choice);
        }
    }
}