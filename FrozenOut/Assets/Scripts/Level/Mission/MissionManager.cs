using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Mission
{
    public class MissionManager : MonoBehaviour
    {
        public List<MissionInfo> Missions;

        public bool IsMissionDone(MissionBase mission)
        {
            MissionInfo missionInfo = GetMission(mission);

            bool state = false;
            if(missionInfo != null)
            {
                state = missionInfo.IsDone;
            }

            return state;
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