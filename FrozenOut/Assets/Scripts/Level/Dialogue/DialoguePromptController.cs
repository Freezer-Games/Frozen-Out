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
        
        private bool IsOpen => CandidateActer != null;

        private DialogueActer CandidateActer;
        private ICollection<DialogueActer> Candidates = new HashSet<DialogueActer>();

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
                    DialogueManager.StartDialogue(CandidateActer);
                }
            }
        }

        public void Open(DialogueActer acter)
        {
            acter.OnDeselected();
            if (CandidateActer == null)
            {
                SetCandidate(acter);
            }
            Candidates.Add(acter);
            // Enable potential canvas
        }

        public void Close(DialogueActer acter)
        {
            Candidates.Remove(acter);
            if (CandidateActer.Equals(acter))
            {
                CandidateActer = null;
                if (Candidates.Count() == 1)
                {
                    SetCandidate(Candidates.First());
                }
                else if (Candidates.Count() > 1)
                {
                    UpdateCandidate();
                }
            }
            // Disable potential canvas
        }

        private void SetCandidate(DialogueActer acter)
        {
            CandidateActer = acter;

            CandidateActer.OnSelected();
        }

        private void UpdateCandidate()
        {
            DialogueActer currentCandidate = CandidateActer;
            float currentDistance = CalculateDistanceToPlayer(CandidateActer);
            foreach (DialogueActer acter in Candidates)
            {
                float otherDistance = CalculateDistanceToPlayer(acter);

                if (otherDistance < currentDistance)
                {
                    currentCandidate = acter;
                    currentDistance = otherDistance;
                }
            }

            if(CandidateActer != currentCandidate) // Si se ha encontrado un candidato más cercano
            {
                CandidateActer?.OnDeselected();
                SetCandidate(currentCandidate);
            }
        }

        private float CalculateDistanceToPlayer(DialogueActer acter)
        {
            float distance = float.PositiveInfinity;
            if(acter != null)
            {
                distance = CalculateSqrMagnitudeDistanceToPlayer(acter.transform.position);
            }

            return distance;
        }

        private float CalculateSqrMagnitudeDistanceToPlayer(Vector3 position)
        {
            Vector3 directionToPlayer = position - DialogueManager.GetPlayer().transform.position;
            float sqrMagnitureDistance = directionToPlayer.sqrMagnitude;

            return sqrMagnitureDistance;
        }
    }
}