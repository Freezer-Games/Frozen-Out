using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Mission
{
    public class MissionManager : MonoBehaviour
    {
        public List<MissionInfo> Missions
        {
            get;
            private set;
        }

        public bool IsMissionDone(MissionBase mission)
        {
            MissionInfo missionInfo = GetMission(mission);

            return missionInfo.IsDone;
        }

        public MissionInfo GetMission(MissionBase mission)
        {
            return Missions.Find(temp => temp.Equals(mission));
        }

        public void MarkMissionAsDone(MissionBase mission)
        {
            MissionInfo missionInfo = GetMission(mission);

            missionInfo.IsDone = true;
        }
    }
}