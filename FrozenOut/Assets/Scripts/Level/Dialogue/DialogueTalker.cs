using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    [RequireComponent(typeof(DialogueIndicator))]
    public class DialogueTalker : MonoBehaviour
    {
        public string Name = "";
        public string TalkToNode = "";

        private DialogueIndicator Indicator;
        public DialogueStyle Style
        {
            get;
            private set;
        }

        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        void Start()
        {
            if(Name.Equals(""))
            {
                Name = gameObject.name;
            }
            Indicator = GetComponent<DialogueIndicator>();
            Style = GetComponent<DialogueStyle>();
        }

        public void OnStartTalk()
        {
            Indicator.HideIndicator();
            transform.LookAt(LevelManager.GetPlayerManager().Player.transform);
        }

        public void OnEndTalk()
        {
            Indicator.ShowIndicator();
        }

        public void OnPlayerClose()
        {
            Indicator.ShowIndicator();
        }

        public void OnPlayerAway()
        {
            Indicator.HideIndicator();
        }

    }
}