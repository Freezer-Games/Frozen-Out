using System;
using UnityEngine;

using Scripts.Level.NPC;

namespace Scripts.Level.Dialogue
{
    public class DialoguePalanquilla : DialogueTalker
    {
        public PoloInfo PoloInfo;

        public override void OnStartTalk()
        {
            base.OnStartTalk();

            PoloInfo.StopAnimation();
        }

        public override void OnEndTalk()
        {
            base.OnEndTalk();

            PoloInfo.StartAnimation("DenyIntervals");
        }
    }
}