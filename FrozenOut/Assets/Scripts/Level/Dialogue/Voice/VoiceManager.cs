using Scripts.Level.Dialogue.Voice;
using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class VoiceManager : MonoBehaviour
    {
        public abstract void Open();
        public abstract void Close();

        public abstract void SetStyle(VoiceStyle style);

        public abstract void Speak(string dialogue);
        public abstract void Speak(char newDialogueLetter);
    }
}