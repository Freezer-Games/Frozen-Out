using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue.System.Choice
{
    public class ButtonChoiceController : UIController
    {
        public ButtonChoiceSystem ChoiceSystem;

        public GameObject ButtonHolder;
        public GameObject ButtonPrefab;

        public override void Open()
        {
            base.Open();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void Close()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            base.Close();
        }

        public void SetButtons(IEnumerable<DialogueChoice> choices)
        {
            foreach (DialogueChoice choice in choices)
            {
                GameObject buttonObject = GameObject.Instantiate(ButtonPrefab, ButtonHolder.transform);

                ButtonChoice buttonChoice = buttonObject.GetComponent<ButtonChoice>();
                buttonChoice.SetButton(choice, this);
            }
        }

        public void SelectChoice(DialogueChoice choiceSelected)
        {
            ChoiceSystem.SelectChoice(choiceSelected);

            foreach (Transform child in ButtonHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}