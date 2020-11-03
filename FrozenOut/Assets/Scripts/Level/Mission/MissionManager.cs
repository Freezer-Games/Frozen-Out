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

            return missionInfo.IsDone;
        }

        public MissionInfo GetMission(MissionBase mission)
        {
            return Missions.Find(temp => temp.Equals(mission));
        }

        public MissionInfo GetActiveMission()
        {
            return Missions.Find(temp => temp.IsActive());
        }

        public MissionInfo SetActiveMission(MissionBase mission)
        {
            Missions.Find(temp => temp.IsActive()).SetInactive();
            MissionInfo ans = Missions.Find(temp => temp.IsActive());
            ans.SetActive();
            return ans;
        }

        public void MarkMissionAsDone(MissionBase mission)
        {
            MissionInfo missionInfo = GetMission(mission);

            missionInfo.IsDone = true;
        }
    }
}