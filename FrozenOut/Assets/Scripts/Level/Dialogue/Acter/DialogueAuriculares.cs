using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueAuriculares : DialogueAnnouncer
    {
        protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        public override void OnEndTalk()
        {
            LevelManager.GetNPCManager().StopAnimationsWithSimilarName("Auriculares");
        }
    }
}