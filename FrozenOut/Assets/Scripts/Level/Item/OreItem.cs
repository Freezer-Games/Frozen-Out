using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class OreItem : ItemUser
    {
        public ParticleSystem Particles;

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
            Debug.Log("Picando");
            StartCoroutine(PlayParticles());
        }

        IEnumerator PlayParticles()
        {
            yield return new WaitForSeconds(0.3f);
            Particles.Play();
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
            yield return null;
        }
    }
}
