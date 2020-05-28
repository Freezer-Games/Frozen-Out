using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class SnowdriftItem : ItemUser
    {
        ParticleSystem Particles;
        
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
            yield return new WaitForSeconds(1f);
            Particles.Play();
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            yield return null;
        }
    }
}
