using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource Steps;
        public AudioSource DialogueText;

        public AudioSource[] MusicSources;
        public AudioSource[] SoundSources;
        // TODO

        public float VolumeReduceFactor = 0.4f;

        public void DecreaseVolume()
        {
            foreach(AudioSource sound in MusicSources)
            {
                sound.volume /= VolumeReduceFactor;
            }
        }

        public void IncreaseVolume()
        {
            foreach(AudioSource sound in MusicSources)
            {
                sound.volume *= VolumeReduceFactor;
            }
        }

    }
}