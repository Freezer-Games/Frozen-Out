using System.Collections;
using UnityEngine;

using Scripts.Level.Sound;

namespace Scripts.Level.Item 
{
    public class SnowdriftItem : ItemUser
    {
        public ParticleSystem Particles;
        public ScoopSoundController SoundController;

        [SerializeField] float animDelay;
        [SerializeField] float particlesDelay = 1f;

        public override void OnPlayerAway()
        {

        }

        public override void OnPlayerClose()
        {
        }

        public override void OnPlayerCol()
        {

        }

        public override void OnPlayerExitCol()
        {
               
        }

        public override void OnUse()
        {
            StartCoroutine(PlayParticles());
        }

        IEnumerator PlayParticles()
        {
            yield return new WaitForSeconds(animDelay);
            Particles.Play();
            SoundController.PlayRandomClip(SoundController.Scoops);
            yield return new WaitForSeconds(particlesDelay);
            DestroyItem();
            yield return null;
        }
    }
}


