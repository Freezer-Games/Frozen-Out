using Cinemachine;
using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public class DialogueOuterTalker : DialogueTalker
    {
        public CinemachineVirtualCamera OtherCamera;

        public override void OnStartTalk()
        {
            OtherCamera.Priority = 50;
            GetComponent<Collider>().enabled = false;
        }

        public override void OnEndTalk()
        {
            OtherCamera.Priority = 0;
        }
    }
}