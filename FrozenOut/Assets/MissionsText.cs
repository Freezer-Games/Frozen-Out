using Scripts.Level.Mission;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Player
{
    public class MissionsText : BaseManager
    {

        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;
        private MissionManager missionsManager;
        MissionBase m;

        Text MText;

        // Start is called before the first frame update
        void Start()
        {

            MText = gameObject.GetComponent<Text>();
            missionsManager = LevelManager.GetMissionManager();
            MText.text = missionsManager.GetActiveMission().Description;
        }

        public void MissionFinished() {
            m = new MissionBase();
            m.VariableName = "IceGot";
            m = missionsManager.GetMission(m);
            m.SetActive();
            MText.text = m.Description;
        }
    }
}
