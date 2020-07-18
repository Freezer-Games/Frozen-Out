using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Mission
{
    public class MissionDoner : MonoBehaviour
    {
        public MissionBase Mission;

        private MissionManager MissionManager => GameManager.Instance.CurrentLevelManager.GetMissionManager();

        public void MarkMissionDone()
        {
            MissionManager.MarkMissionAsDone(Mission);
        }
    }
}