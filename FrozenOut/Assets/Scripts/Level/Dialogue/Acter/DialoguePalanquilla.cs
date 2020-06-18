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
            Indicator.HideIndicator();

            StopAllCoroutines();

            PoloInfo.StopAnimation();
        }

        public override void OnEndTalk()
        {
            Indicator.ShowIndicator();

            StopAllCoroutines();

            PoloInfo.StartAnimation("DenyIntervals");
        }
    }
}