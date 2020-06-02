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
        private ICollection<DialogueTalker> Candidates = new HashSet<DialogueTalker>();

        void Update()
        {
            if(IsOpen)
            {
                if(Candidates.Count() > 1)
                {
                    UpdateCandidate();
                }
                if(DialogueManager.IsReady() && Input.GetKey(DialogueManager.GetInteractKey()))
                {
                    DialogueManager.StartDialogue(CandidateTalker);
                }
            }
        }

        public void Open(DialogueTalker talker)
        {
            if(CandidateTalker == null)
            {
                SetCandidate(talker);
            }
            Candidates.Add(talker);
            // Enable potential canvas
        }

        public void Close(DialogueTalker talker)
        {
            Candidates.Remove(talker);
            if(CandidateTalker.Equals(talker) || Candidates.Count() == 0)
            {
                CandidateTalker = null;
            }
            // Disable potential canvas
        }

        private void SetCandidate(DialogueTalker talker)
        {
            CandidateTalker = talker;

            CandidateTalker.OnSelected();
        }

        private void UpdateCandidate()
        {
            DialogueTalker currentCandidate = CandidateTalker;
            float currentDistance = CalculateSqrMagnitudeDistanceToPlayer(CandidateTalker.transform.position);
            foreach (DialogueTalker talker in Candidates)
            {
                float otherDistance = CalculateSqrMagnitudeDistanceToPlayer(talker.transform.position);

                if (otherDistance < currentDistance)
                {
                    currentCandidate = talker;
                    currentDistance = otherDistance;
                }
            }

            if(CandidateTalker != currentCandidate)
            {
                CandidateTalker.OnDeselected();
                SetCandidate(currentCandidate);
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