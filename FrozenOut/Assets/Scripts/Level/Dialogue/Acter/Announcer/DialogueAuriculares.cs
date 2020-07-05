using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueAuriculares : DialogueAnnouncer
    {
        public override void OnEndTalk()
        {
            LevelManager.GetNPCManager().StopAnimationsWithSimilarName("Auriculares");
        }
    }
}