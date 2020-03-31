using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Level.Animation
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(LookAt))]
    public class LocomotionSimpleAgent : MonoBehaviour
    {

        public string Animation;

        private Animator Animator;
        private NavMeshAgent Agent;
        private Vector2 SmoothDeltaPosition = Vector2.zero;
        private Vector2 Velocity = Vector2.zero;
        private LookAt LookAt;

        void Start()
        {
            Animator = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            LookAt = GetComponent<LookAt>();

            // Don’t update position automatically
            Agent.updatePosition = false;
        }

        void Update()
        {
            Vector3 worldDeltaPosition = Agent.nextPosition - transform.position;

            // Map worldDeltaPosition to local space
            float deltaX = Vector3.Dot(transform.right, worldDeltaPosition);
            float deltaY = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(deltaX, deltaY);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                Velocity = SmoothDeltaPosition / Time.deltaTime;

            bool shouldMove = Velocity.magnitude > 0.5f && Agent.remainingDistance > Agent.radius;

            // Update animation parameters
            Animator.SetBool(Animation, shouldMove);

            LookAt.UpdateLookAtTargetPosition(Agent.steeringTarget + transform.forward);
        }

        void OnAnimatorMove()
        {
            // Update position to agent position
            transform.position = Agent.nextPosition;
        }

    }
}