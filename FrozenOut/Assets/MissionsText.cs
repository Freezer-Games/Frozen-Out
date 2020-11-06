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


        public GameObject MissionPanel;
        public Text MissionText;
        public GameObject MissionObject;

        private LocalizedString LocalizeScriptName => MissionObject.GetComponent<LocalizeStringBehaviour>().StringReference;

        private void Start()
        {
            MissionManager.NewEventActive += (sender, args) => { StopAllCoroutines(); StartCoroutine(DoShowNewText()); };
            
            StartCoroutine(DoShowText());
        }

        private void Update()
        {
            if (Input.GetKey(SettingsManager.MissionsKey))
            {
                StopAllCoroutines();
                StartCoroutine(DoShowText());
            }
        }

        private IEnumerator DoShowNewText()
        {
            MissionPanel.SetActive(true);
            MissionText.color = Color.red;

            yield return new WaitForSeconds(1);

            yield return StartCoroutine(DoShowText());
        }

        private IEnumerator DoShowText()
        {
            MissionPanel.SetActive(true);
            MissionText.color = Color.white;

            MissionInfo activeMission = MissionManager.GetActiveMission();
            if (activeMission != null)
            {
                LocalizeScriptName.TableEntryReference = activeMission.VariableName;
            }
            else
            {
                LocalizeScriptName.TableEntryReference = "empty";
            }
            yield return new WaitForSeconds(5);

            MissionPanel.SetActive(false);
            yield return null;
        }
    }
}
