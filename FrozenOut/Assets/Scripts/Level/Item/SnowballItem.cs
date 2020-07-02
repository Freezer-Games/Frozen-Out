using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Scripts.Level.Item
{
    public class SnowballItem : ItemUser
    {
        public bool readyToPlay;
        public PlayableDirector Timeline;

        public override void OnPlayerCol() {}

        public override void OnPlayerExitCol() {}

        public override void OnUse()
        {
            if (readyToPlay)
            {
                Timeline.Play();
                DestroyItem();
            }
        }
    }
}
