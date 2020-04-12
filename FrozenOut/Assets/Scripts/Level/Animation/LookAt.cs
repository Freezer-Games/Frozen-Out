using System.Collections;
using UnityEngine;

namespace Scripts.Level.Animation
{
    [RequireComponent(typeof(Animator))]
    public class LookAt : MonoBehaviour
    {
        
        public Transform Head = null;
        public Vector3 LookAtTargetPosition;
        public float LookAtCoolTime = 0.2f;
        public float LookAtHeatTime = 0.2f;
        public bool IsLooking
        {
            get;
            private set;
        } = true;

        private Vector3 LookAtPosition;
        private Animator Animator;
        private float LookAtWeight = 0.0f;

        void Start()
        {
            if (!Head)
            {
                Debug.LogError("No head transform - LookAt disabled");
                enabled = false;
                return;
            }
            
            Animator = GetComponent<Animator>();
            LookAtTargetPosition = Head.position + transform.forward;
            LookAtPosition = LookAtTargetPosition;
        }

        void OnAnimatorIK()
        {
            LookAtTargetPosition.y = Head.position.y;
            float lookAtTargetWeight = IsLooking? 1.0f : 0.0f;

            Vector3 curDir = LookAtPosition - Head.position;
            Vector3 futDir = LookAtTargetPosition - Head.position;

            curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
            LookAtPosition = Head.position + curDir;

            float blendTime = (lookAtTargetWeight > LookAtWeight)? LookAtHeatTime : LookAtCoolTime;
            LookAtWeight = Mathf.MoveTowards(LookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
            Animator.SetLookAtWeight(LookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
            Animator.SetLookAtPosition(LookAtPosition);
        }

        public void UpdateLookAtTargetPosition(Vector3 newPosition)
        {
            LookAtTargetPosition = newPosition;
        }

    }
}