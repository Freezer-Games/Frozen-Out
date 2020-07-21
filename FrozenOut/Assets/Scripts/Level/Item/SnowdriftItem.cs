using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

using Scripts.Level.Sound;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item 
{
    public class SnowdriftItem : ItemUser
    {
        public ScoopSoundController SoundController;
        public DialogueActer UnableTalker;
        public Collider Collider;
        public bool End;

        public bool TriggerTimeline;
        public PlayableDirector Timeline;

        [SerializeField] float animDelay;

        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        public override void OnPlayerCol() {}

        public override void OnPlayerExitCol() {}

        public override void OnUse()
        {
            if (TriggerTimeline)
            {
                HighlightItem(false);
                Collider.enabled = false;
                Timeline.Play();
            }
            else
            {
                StartCoroutine(Destroy());
            }
            
            if (End)
            {
                DestroyItem();
            }
            
        }

        public override void OnUnableUse()
        {
            DialogueManager.StartDialogue(UnableTalker);
        }

        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(animDelay);
            SoundController.PlayRandomClip(SoundController.Scoops);
            DestroyItem();
        }
    }
}


