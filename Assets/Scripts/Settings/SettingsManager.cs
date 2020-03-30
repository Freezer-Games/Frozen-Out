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

        public KeyCode [] MovementKeys => new KeyCode [] { ForwardKey, BackwardKey, LeftKey, RightKey };

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

        public string Language
        {
            get;
            private set;
        }
        public List<string> SupportedLanguages
        {
            get;
            private set;
        }

        public Dictionary<string, List<string>> SupportedResolutions
        {
            get;
            private set;
        }
        public string Resolution
        {
            get;
            private set;
        }

        public List<string> SupportedAspectRatios
        {
            get;
            private set;
        }
        public string AspectRatio
        {
            get;
            private set;
        }

        public List<string> SupportedScreenTypes
        {
            get;
            private set;
        }
        public string ScreenType
        {
            get;
            private set;
        }
        
        public string Quality
        {
            get;
            private set;
        }

        private GameManager GameManager => GameManager.Instance;

        void Awake()
        {
            AssignKeys();

            TextSize = PlayerPrefs.GetFloat(nameof(TextSize), 14);
            MusicVolume = PlayerPrefs.GetFloat(nameof(MusicVolume), 100);
            SoundVolume = PlayerPrefs.GetFloat(nameof(SoundVolume), 100);

            AssignLanguages();
            AssignGraphics();
        }

        public void SetForwardKey(KeyCode keyCode)
        {
            ForwardKey = keyCode;
            SetPlayerPrefsKey(nameof(ForwardKey), keyCode);
        }

        public void SetBackwardKey(KeyCode keyCode)
        {
            BackwardKey = keyCode;
            SetPlayerPrefsKey(nameof(BackwardKey), keyCode);
        }

        public void SetRightKey(KeyCode keyCode)
        {
            RightKey = keyCode;
            SetPlayerPrefsKey(nameof(RightKey), keyCode);
        }

        public void SetLeftKey(KeyCode keyCode)
        {
            LeftKey = keyCode;
            SetPlayerPrefsKey(nameof(LeftKey), keyCode);
        }

        public void SetJumpKey(KeyCode keyCode)
        {
            JumpKey = keyCode;
            SetPlayerPrefsKey(nameof(JumpKey), keyCode);
        }

        public void SetCrouchKey(KeyCode keyCode)
        {
            CrouchKey = keyCode;
            SetPlayerPrefsKey(nameof(CrouchKey), keyCode);
        }

        public void SetInteractKey(KeyCode keyCode)
        {
            InteractKey = keyCode;
            SetPlayerPrefsKey(nameof(InteractKey), keyCode);
        }

        public void SetMissionsKey(KeyCode keyCode)
        {
            MissionsKey = keyCode;
            SetPlayerPrefsKey(nameof(MissionsKey), keyCode);
        }

        public void SetNextDialogueKey(KeyCode keyCode)
        {
            NextDialogueKey = keyCode;
            SetPlayerPrefsKey(nameof(NextDialogueKey), keyCode);
        }

        public void SetMusicVolume(float newVolume)
        {
            MusicVolume = newVolume;
            PlayerPrefs.SetFloat(nameof(MusicVolume), newVolume);
        }

        public void SetSoundVolume(float newVolume)
        {
            SoundVolume = newVolume;
            PlayerPrefs.SetFloat(nameof(SoundVolume), newVolume);
        }

        public void SetTextSize(float newTextSize)
        {
            TextSize = newTextSize;
            PlayerPrefs.SetFloat(nameof(TextSize), newTextSize);
        }

        public void SetLanguage(string newLanguage)
        {
            Language = newLanguage;
            PlayerPrefs.SetString(nameof(Language), newLanguage);
        }

        public void SetAspectRatio(string newAspectRatio)
        {
            AspectRatio = newAspectRatio;
            PlayerPrefs.SetString(nameof(AspectRatio), newAspectRatio);
        }

        public void SetResolution(string newResolution)
        {
            Resolution = newResolution;
            PlayerPrefs.SetString(nameof(Resolution), newResolution);
        }

        public void SetScreenType(string newScreenType)
        {
            ScreenType = newScreenType;
            PlayerPrefs.SetString(nameof(ScreenType), newScreenType);
        }

        public void SetLowQuality()
        {
            SetQuality("Low");
        }

        public void SetMediumQuality()
        {
            SetQuality("Medium");
        }

        public void SetHighQuality()
        {
            SetQuality("High");
        }
        
        private void SetQuality(string newQuality)
        {
            Quality = newQuality;
            PlayerPrefs.SetString(nameof(Quality), newQuality);
        }

        private void AssignLanguages()
        {
            SupportedLanguages = new List<string>();
            SupportedLanguages.Add(SystemLanguage.Spanish.ToString());
            SupportedLanguages.Add(SystemLanguage.English.ToString());

            Language = PlayerPrefs.GetString(nameof(Language), SupportedLanguages[0]);
        }

        private void AssignGraphics()
        {
            SupportedAspectRatios = new List<string>();
            SupportedAspectRatios.Add("16:9");
            SupportedAspectRatios.Add("4:3");
            SupportedAspectRatios.Add("1:1");
            SupportedAspectRatios.Add("18:5");
            AspectRatio = PlayerPrefs.GetString(nameof(AspectRatio), SupportedAspectRatios[0]);

            SupportedResolutions = new Dictionary<string, List<string>>();

            List<string> resolutions16_9 = new List<string>();
            resolutions16_9.Add("1280x720");
            resolutions16_9.Add("1920x1080");
            resolutions16_9.Add("2560x1440");
            SupportedResolutions.Add("16:9", resolutions16_9);

            List<string> resolutions4_3 = new List<string>();
            resolutions4_3.Add("640x480");
            resolutions4_3.Add("800x600");
            resolutions4_3.Add("960x720");
            resolutions4_3.Add("1024x768");
            resolutions4_3.Add("1280x960");
            resolutions4_3.Add("1400x1050");
            resolutions4_3.Add("1440x1080");
            SupportedResolutions.Add("4:3", resolutions4_3);

            List<string> resolutions1_1 = new List<string>();
            resolutions1_1.Add("600x600");
            resolutions1_1.Add("720x720");
            resolutions1_1.Add("800x800");
            resolutions1_1.Add("1080x1080");
            resolutions1_1.Add("1440x1440");
            SupportedResolutions.Add("1:1", resolutions1_1);

            List<string> resolutions18_5 = new List<string>();
            resolutions18_5.Add("3840x1080");
            resolutions18_5.Add("5120x1440");
            SupportedResolutions.Add("18:5", resolutions18_5);

            Resolution = PlayerPrefs.GetString(nameof(Resolution), SupportedResolutions[SupportedAspectRatios[0]][0]);

            SupportedScreenTypes = new List<string>();
            SupportedScreenTypes.Add("Windowed");
            SupportedScreenTypes.Add("Fullscreen");
            ScreenType = PlayerPrefs.GetString(nameof(ScreenType), SupportedScreenTypes[0]);

            Quality = PlayerPrefs.GetString(nameof(Quality), "High");
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

        private void SetPlayerPrefsKey(string name, KeyCode keyCode)
        {
            PlayerPrefs.SetString(name, keyCode.ToString());
        }

    }
}