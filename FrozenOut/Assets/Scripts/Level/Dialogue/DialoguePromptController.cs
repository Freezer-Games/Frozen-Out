using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Scripts.Level.Dialogue
{
    public class DialoguePromptController : MonoBehaviour
    {
        public DialogueManager DialogueManager;
        
        private bool IsOpen => CandidateTalker != null;

        private DialogueTalker CandidateTalker;
        private float CandidateDistance;

        void Update()
        {
            if(IsOpen && DialogueManager.IsReady() && Input.GetKey(DialogueManager.GetInteractKey()))
            {
                DialogueManager.StartDialogue(CandidateTalker);
            }
        }

        public void Open(DialogueTalker talker)
        {
            if(CandidateTalker == null)
            {
                SetCandidate(talker);
            }
            else
            {
                if(!CandidateTalker.Equals(talker)) // Si es otro que esta cerca y le puede hacer competencia
                {
                    UpdateCandidate(talker);
                }
            }
            // Enable potential canvas
        }

        public void Close(DialogueTalker talker)
        {
            if(CandidateTalker != null && CandidateTalker.Equals(talker))
            {
                CandidateTalker = null;
                CandidateDistance = Mathf.Infinity;
            }
            // Disable potential canvas
        }

        private void SetCandidate(DialogueTalker talker, float distance = Mathf.Infinity)
        {
            CandidateTalker = talker;
            CandidateDistance = distance;

            CandidateTalker.OnSelected();
        }

        private void UpdateCandidate(DialogueTalker otherTalker)
        {
            CandidateDistance = CalculateSqrMagnitudeDistanceToPlayer(CandidateTalker.transform.position);

            float otherDistance = CalculateSqrMagnitudeDistanceToPlayer(otherTalker.transform.position);

            if(otherDistance <= CandidateDistance)
            {
                CandidateTalker.OnDeselected();
                SetCandidate(otherTalker, otherDistance);
            }
        }

        private float CalculateSqrMagnitudeDistanceToPlayer(Vector3 position)
        {
            Vector3 directionToPlayer = position - DialogueManager.GetPlayer().transform.position;
            float sqrMagnitureDistance = directionToPlayer.sqrMagnitude;

            return sqrMagnitureDistance;
        }
    }
}