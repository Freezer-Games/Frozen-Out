using UnityEngine;

namespace Scripts.Level.Sound
{
    public class OreSoundController : SoundController
    {
        public AudioClip[] Ores;

        public void PlayOreSound()
        {
            PlayRandomClip(Ores);
        }
    }
}