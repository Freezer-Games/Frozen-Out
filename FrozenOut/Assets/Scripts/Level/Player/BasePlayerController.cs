using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player 
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BasePlayerController: MonoBehaviour
    {
        public PlayerManager PlayerManager;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public Collider Collider;

        protected Vector2 MoveInput;
        protected Vector3 Movement;
        protected Vector3 CamForward;
        protected Vector3 CamRight;

        protected abstract void Move();

        public void MoveToTarget(Transform target, float distanceToStop, float speed)
        {
            Vector3 lookPos = target.position;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);

            if (Vector3.Distance(target.position, transform.position) > distanceToStop) 
            {
                Animator.SetBool("isMoving", true);

                transform.position = 
                    Vector3.MoveTowards(
                        transform.position, 
                        target.position, 
                        speed * Time.fixedDeltaTime);
            }
            else 
            {
                Animator.SetBool("isMoving", false);
            }
        }

        public void FaceMovement() 
        {
            transform.rotation = 
                Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation(Movement.normalized), 
                    0.2f);
        }

        protected void CameraVectors() 
        {
            CamForward = UnityEngine.Camera.main.transform.forward;
            CamRight = UnityEngine.Camera.main.transform.right;
            CamForward.y = 0f;
            CamRight.y = 0f;
            CamForward.Normalize();
            CamRight.Normalize();
        }
    }
}

