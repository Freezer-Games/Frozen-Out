using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueAnnouncer : DialogueActer
    {
        void Awake()
        {
            SetNonBlocking();
            SetAutomatic();
        }

        public override void OnStartTalk() { }

        public override void OnEndTalk() { }

        public override void OnPlayerClose() { }

        public override void OnPlayerAway() { }

        public override void OnSelected() { }

        public override void OnDeselected() { }
    }
}