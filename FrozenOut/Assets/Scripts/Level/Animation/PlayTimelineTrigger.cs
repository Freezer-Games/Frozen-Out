using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Scripts.Level.Animation
{
    public class PlayTimelineTrigger : TriggerBase
    {
        public PlayableDirector Timeline;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
            {
                GetComponent<Collider>().enabled = false;
                Timeline.Play();
            }
        }
    }
}