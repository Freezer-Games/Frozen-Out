using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        public KeyCode ForwardKey
        {
            get;
            private set;
        }
        public KeyCode BackwardKey
        {
            get;
            private set;
        }
        public KeyCode RightKey
        {
            get;
            private set;
        }
        public KeyCode LeftKey
        {
            get;
            private set;
        }

        public KeyCode[] MovementKeys => new KeyCode[] { ForwardKey, BackwardKey, LeftKey, RightKey };

        public KeyCode JumpKey
        {
            get;
            private set;
        }
        public KeyCode CrouchKey
        {
            get;
            private set;
        }

        public KeyCode InteractKey
        {
            get;
            private set;
        }
        public KeyCode NextDialogueKey
        {
            get;
            private set;
        }
        public KeyCode MissionsKey
        {
            get;
            private set;
        }
        public KeyCode PauseKey
        {
            get;
            private set;
        }

        public float MusicVolume
        {
            get;
            private set;
        }
        public float SoundVolume
        {
            get;
            private set;
        }
        public float TextSize
        {
            get;
            private set;
        }

        void Awake()
        {
            AssignKeys();
            TextSize = PlayerPrefs.GetFloat("TextSize", 14);
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 100);
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 100);
        }

        private void AssignKeys()
        {
            JumpKey = GetPlayerPrefsKey(nameof(JumpKey), "Space");
            ForwardKey = GetPlayerPrefsKey(nameof(ForwardKey), "W");
            BackwardKey = GetPlayerPrefsKey(nameof(BackwardKey), "S");
            RightKey = GetPlayerPrefsKey(nameof(RightKey), "D");
            LeftKey = GetPlayerPrefsKey(nameof(LeftKey), "A");
            CrouchKey = GetPlayerPrefsKey(nameof(CrouchKey), "LeftControl");
            InteractKey = GetPlayerPrefsKey(nameof(InteractKey), "F");
            NextDialogueKey = GetPlayerPrefsKey(nameof(NextDialogueKey), "Space");
            MissionsKey = GetPlayerPrefsKey(nameof(MissionsKey), "Tab");
            PauseKey = GetPlayerPrefsKey(nameof(PauseKey), "Escape");
        }

        private KeyCode GetPlayerPrefsKey(string name, string defaultValue)
        {
            string stringPref = PlayerPrefs.GetString(name, defaultValue);
            KeyCode keyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), stringPref);
            return keyCode;
        }
    }
}