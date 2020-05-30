using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class OreItem : ItemUser
    {
        public ParticleSystem Particles;
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
            yield return new WaitForSeconds(particlesDelay);
            DestroyItem();
            yield return null;
        }
    }
}
