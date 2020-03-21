using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        public KeyCode Forward
        {
            get;
            private set;
        }
        public KeyCode Backward
        {
            get;
            private set;
        }
        public KeyCode Right
        {
            get;
            private set;
        }
        public KeyCode Left
        {
            get;
            private set;
        }

        public KeyCode[] MovementKeys => new KeyCode[] { Forward, Backward, Left, Right };

        public KeyCode Jump
        {
            get;
            private set;
        }
        public KeyCode Crouch
        {
            get;
            private set;
        }

        public KeyCode Interact
        {
            get;
            private set;
        }
        public KeyCode NextDialogue
        {
            get;
            private set;
        }
        public KeyCode Missions
        {
            get;
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
            KeyCode keyCode = (keyCode) System.Enum.Parse(typeof(KeyCode), stringPref);
            return keyCode;
        }
    }
}