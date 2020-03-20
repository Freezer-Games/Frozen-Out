using System.Collections;
using System.Collections.Generic;
using System.Enum;
using UnityEngine;

namespace Scripts.Input
{
    public class InputManager : Monobehaviour
    {
        public KeyCode Forward
        {
            private set;
        }
        public KeyCode Backward
        {
            private set;
        }
        public KeyCode Right
        {
            private set;
        }
        public KeyCode Left
        {
            private set;
        }

        public KeyCode[] MovementKeys => new KeyCode[] { Forward, Backward, Left, Right };

        public KeyCode Jump
        {
            private set;
        }
        public KeyCode Crouch
        {
            private set;
        }

        public KeyCode Interact
        {
            private set;
        }
        public KeyCode NextDialogue
        {
            private set;
        }
        public KeyCode Missions
        {
            private set;
        }

        void Awake()
        {
            AssignKeys();
        }

        private void AssignKeys()
        {
            Jump = GetPlayerPrefsKey("jumpKey", "Space");
            Forward = GetPlayerPrefsKey("forwardKey", "W");
            Backward = GetPlayerPrefsKey("backwardKey", "S");
            Right = GetPlayerPrefsKey("rightKey", "D");
            Left = GetPlayerPrefsKey("leftKey", "A");
            Crouch = GetPlayerPrefsKey("CrouchKey", "LeftControl");
            Interact = GetPlayerPrefsKey("InteractKey", "F");
            NextDialogue = GetPlayerPrefsKey("NextDialogueKey", "Space");
            Missions = GetPlayerPrefsKey("MissionsKey", "Tab");
        }

        private KeyCode GetPlayerPrefsKey(string name, string defaultValue)
        {
            string stringPref = PlayerPrefs.GetString(name, defaultValue);
            KeyCode keyCode = (keyCode) Parse(typeof(KeyCode), stringPref);
            return keyCode;
        }
    }
}