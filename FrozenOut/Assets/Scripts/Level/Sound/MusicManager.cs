using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Sound
{
    public class MusicManager : MonoBehaviour
    {
        public LevelManager LevelManager;

        public AudioSource AudioSource;

        public float VolumeReduceFactor = 0.4f;

        public void DecreaseVolume()
        {
            AudioSource.volume /= VolumeReduceFactor;
        }

        public void IncreaseVolume()
        {
            AudioSource.volume *= VolumeReduceFactor;
        }
    }
}