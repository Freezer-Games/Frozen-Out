using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

using Scripts.Level.Sound;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item 
{
    public class SnowdriftItem : ItemUser
    {
        public ParticleSystem Particles;
        public ScoopSoundController SoundController;
        public DialogueActer UnableTalker;
        public Collider Collider;

        public bool TriggerTimeline;
        public PlayableDirector Timeline;

        [SerializeField] float animDelay;
        [SerializeField] float particlesDelay = 1f;

        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        public override void OnPlayerCol() {}

        public override void OnPlayerExitCol() {}

        public override void OnUse()
        {
            StartCoroutine(PlayParticles());

            if (TriggerTimeline)
            {
                HighlightItem(false);
                Collider.enabled = false;
                Timeline.Play();
            }
        }

        public override void OnUnableUse()
        {
            DialogueManager.StartDialogue(UnableTalker);
        }

        IEnumerator PlayParticles()
        {
            yield return new WaitForSeconds(animDelay);
            Particles.Play();
            SoundController.PlayRandomClip(SoundController.Scoops);
            yield return new WaitForSeconds(particlesDelay);
            Particles.Stop();

            if (!TriggerTimeline)
            {
                DestroyItem();
            }
        }
    }
}


