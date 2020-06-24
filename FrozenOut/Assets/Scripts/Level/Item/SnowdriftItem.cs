using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

using Scripts.Level.Sound;

namespace Scripts.Level.Item 
{
    public class SnowdriftItem : ItemUser
    {
        public ParticleSystem Particles;
        public ScoopSoundController SoundController;

        public bool TriggerTimeline;
        public PlayableDirector Timeline;

        [SerializeField] float animDelay;
        [SerializeField] float particlesDelay = 1f;

        public override void OnPlayerAway() {}

        public override void OnPlayerClose() {}

        public override void OnPlayerCol() {}

        public override void OnPlayerExitCol() {}

        public override void OnUse()
        {
            StartCoroutine(PlayParticles());

            if (TriggerTimeline)
            {
                Timeline.Play();
            }
        }

        IEnumerator PlayParticles()
        {
            yield return new WaitForSeconds(animDelay);
            Particles.Play();
            SoundController.PlayRandomClip(SoundController.Scoops);
            yield return new WaitForSeconds(particlesDelay);
            Particles.Stop();

            if (!TriggerTimeline) DestroyItem();

            yield return null;
        }
    }
}


