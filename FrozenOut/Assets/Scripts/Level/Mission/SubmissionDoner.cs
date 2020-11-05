using UnityEngine;

namespace Scripts.Level.Mission
{
    public class SubmissionDoner : MonoBehaviour
    {
        public MissionBase Submission;

        private MissionManager MissionManager => GameManager.Instance.CurrentLevelManager.GetMissionManager();

        public void MarkSubmissionDone()
        {
            MissionManager.MarkSubmissionAsDone(Submission);
        }
    }
}