using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class ControlsSectionController : MonoBehaviour
    {
        public OptionsMenuController OptionsMenuController;
        private MainMenuManager MainMenuManager => OptionsMenuController.MainMenuManager;

        public Canvas ControlsSectionCanvas;

        public Button ForwardKey;
        public Button BackKey;
        public Button RightKey;
        public Button LeftKey;
        public Button JumpKey;
        public Button CrouchKey;
        public Button InteractKey;
        public Button MissionsKey;
        public Button NextDialogueKey;
        
        Event KeyEvent;
        KeyCode NewKey;
        bool WaitingForKey;

        void Start()
        {
            WaitingForKey = false;

            ForwardKey.GetComponentInChildren<Text>().text = MainMenuManager.GetForwardKey().ToString();
            BackKey.GetComponentInChildren<Text>().text = MainMenuManager.GetBackKey().ToString();
            RightKey.GetComponentInChildren<Text>().text = MainMenuManager.GetRightKey().ToString();
            LeftKey.GetComponentInChildren<Text>().text = MainMenuManager.GetLeftKey().ToString();
            JumpKey.GetComponentInChildren<Text>().text = MainMenuManager.GetJumpKey().ToString();
            CrouchKey.GetComponentInChildren<Text>().text = MainMenuManager.GetCrouchKey().ToString();
            InteractKey.GetComponentInChildren<Text>().text = MainMenuManager.GetInteractKey().ToString();
            MissionsKey.GetComponentInChildren<Text>().text = MainMenuManager.GetMissionsKey().ToString();
            NextDialogueKey.GetComponentInChildren<Text>().text = MainMenuManager.GetNextDialogueKey().ToString();

            ForwardKey.onClick.AddListener(() => StartAssignment(nameof(ForwardKey)));
            BackKey.onClick.AddListener(() => StartAssignment(nameof(BackKey)));
            RightKey.onClick.AddListener(() => StartAssignment(nameof(RightKey)));
            LeftKey.onClick.AddListener(() => StartAssignment(nameof(LeftKey)));
            JumpKey.onClick.AddListener(() => StartAssignment(nameof(JumpKey)));
            CrouchKey.onClick.AddListener(() => StartAssignment(nameof(CrouchKey)));
            InteractKey.onClick.AddListener(() => StartAssignment(nameof(InteractKey)));
            MissionsKey.onClick.AddListener(() => StartAssignment(nameof(MissionsKey)));
            NextDialogueKey.onClick.AddListener(() => StartAssignment(nameof(NextDialogueKey)));
        }

        public void Open()
        {
            ControlsSectionCanvas.enabled = true;
        }

        public void Close()
        {
            ControlsSectionCanvas.enabled = false;
        }

        private void StartAssignment(string keyName)
        {
            if(!WaitingForKey)
            {
                StartCoroutine(AssignKey(keyName));
            }
        }

        //TODO
        
        void OnGUI()
        {
            KeyEvent = Event.current;

            if (WaitingForKey && KeyEvent.isKey)
            {
                NewKey = KeyEvent.keyCode;
                WaitingForKey = false;
            }
        }

        IEnumerator WaitForKey()
        {
            while(!KeyEvent.isKey)
            {
                yield return null;
            }

        }

        public IEnumerator AssignKey(string keyName)
        {
            WaitingForKey = true;

            yield return WaitForKey();
            switch(keyName)
            {
                case nameof(ForwardKey):
                    MainMenuManager.SetForwardKey(NewKey);
                    ForwardKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(BackKey):
                    MainMenuManager.SetBackKey(NewKey);
                    BackKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(RightKey):
                    MainMenuManager.SetRightKey(NewKey);
                    RightKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(LeftKey):
                    MainMenuManager.SetLeftKey(NewKey);
                    LeftKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(JumpKey):
                    MainMenuManager.SetJumpKey(NewKey);
                    JumpKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(CrouchKey):
                    MainMenuManager.SetCrouchKey(NewKey);
                    CrouchKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(InteractKey):
                    MainMenuManager.SetInteractKey(NewKey);
                    InteractKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(MissionsKey):
                    MainMenuManager.SetMissionsKey(NewKey);
                    MissionsKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
                case nameof(NextDialogueKey):
                    MainMenuManager.SetNextDialogueKey(NewKey);
                    NextDialogueKey.GetComponentInChildren<Text>().text = NewKey.ToString();
                    break;
            }

            yield return null;
        }

    }
}