using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Level.Sound
{
    public class PlayerSound : SoundController
    {
        public AudioClip[] SnowFootSteps;

        public void SelectClipToPlay()
        {
            int choosen = Random.Range(0, SnowFootSteps.Length);
            PlayClip(SnowFootSteps[choosen]);
        }
    }
}

