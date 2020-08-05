using Scripts.Level.Dialogue.Voice;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class VoiceManager : MonoBehaviour
    {
        public VoiceStyleConfiguration VoiceConfiguration;

        public abstract void Open();
        public abstract void Close();

        public abstract void StartLine();

        public abstract void SetStyle(VoiceStyle style);
        public abstract void SpeakDialogueAccumulated(string dialogue);
        public abstract void SpeakDialogueSingle(string dialogueLetter);
    }
}