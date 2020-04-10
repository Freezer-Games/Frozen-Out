using System;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueTalker : MonoBehaviour
    {

        public string Name = "";
        public string TalkToNode = "";

        private DialogueIndicator Indicator;

        void Start()
        {
            Indicator = GetComponent<DialogueIndicator>();
        }

        public void OnStartTalk()
        {
            Indicator.HideIndicator();
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