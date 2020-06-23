using UnityEngine;

namespace Scripts.Level.Sound
{
    public class PlayerSound : SoundController
    {
        public AudioClip[] SnowFootSteps;

        public void SelectClipToPlay()
        {
            PlayRandomClip(SnowFootSteps);
        }
    }
}

