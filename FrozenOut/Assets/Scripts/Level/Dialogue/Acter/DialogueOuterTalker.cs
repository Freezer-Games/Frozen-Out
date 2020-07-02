using Cinemachine;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueOuterTalker : DialogueActer
    {
        public CinemachineVirtualCamera OtherCamera;

        void Start()
        {
            SetBlocking();
            SetAutomatic();
        }

        public override void OnStartTalk()
        {
            OtherCamera.Priority = 50;
            GetComponent<Collider>().enabled = false;
        }

        public override void OnEndTalk()
        {
            OtherCamera.Priority = 0;
        }

        public override void OnPlayerClose()
        {
            
        }

        public override void OnPlayerAway()
        {
            
        }

        public override void OnSelected()
        {
            
        }

        public override void OnDeselected()
        {
            
        }
    }
}