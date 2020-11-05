using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

using Scripts.Level.Mission;
using Scripts.Settings;

namespace Scripts.Level.Player
{
    public class MissionsText : BaseManager
    {
        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;
        private MissionManager MissionManager => LevelManager.GetMissionManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        public GameObject MissionObject;
        private LocalizedString LocalizeScriptName => MissionObject.GetComponent<LocalizeStringBehaviour>().StringReference;

        Text MissionText;

        private void Start()
        {
            MissionText = GetComponent<Text>();
        }

        private void Update()
        {
            if (Input.GetKey(SettingsManager.MissionsKey))
            {
                StartCoroutine(DoShowText());
            }
        }

        private IEnumerator DoShowText()
        {
            MissionText.enabled = true;

            MissionInfo activeMission = MissionManager.GetActiveMission();
            if (activeMission != null)
            {
                LocalizeScriptName.TableEntryReference = activeMission.VariableName;
            }
            else
            {
                LocalizeScriptName.TableEntryReference = "empty";
            }
            yield return new WaitForSeconds(10);
            MissionText.enabled = false;
            yield return null;
        }
    }
}
