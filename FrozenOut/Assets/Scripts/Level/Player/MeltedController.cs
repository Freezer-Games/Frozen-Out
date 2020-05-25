using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Player
{
    public class MeltedController : BasePlayerController
    {
        [Header("States")]
        public bool IsInteracting;
        [SerializeField] bool CanMove;
        [SerializeField] bool IsMoving;
        [SerializeField] bool IsCharging;


        [Header("Movement")]
        [SerializeField] float MoveSpeed = 4f;
        [SerializeField] float MovementDelay;


        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;

        public UnityEvent Recovery;

        void Start() 
        {
            CanMove = true;
            IsInteracting = false;
            IsMoving = false;
            Collider.enabled = true;

            if (Recovery == null) 
                Recovery = new UnityEvent();
        }

        void Update()
        {
            if (PlayerManager.IsEnabled)
            {
                if (IsInteracting) 
                {
                    CanMove = false;
                    Rigidbody.isKinematic = true;
                    MoveToTarget(InteractPos, InteractLook, 0.01f, 0.5f);
                }
                else
                {
                    CanMove = true;
                    Rigidbody.isKinematic = false;
                }

                if (CanMove)
                {
                    MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));
                    MoveInput.Normalize();

                    Animator.SetBool("isMoving", MoveInput != Vector2.zero);

                    if (MoveInput != Vector2.zero)
                    { 
                        FaceMovement();
                    }
                }
            }
        }

        void FixedUpdate() 
        {
            if (CanMove)
            {
                Move();
            }
        }

        void LateUpdate() 
        {
            CameraVectors();
        }

        void OnDisablae() 
        {
            Debug.Log("Desactivnado collider");
            Collider.enabled = false;
        }

        protected override void Move()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
            

            if (IsCharging) 
            {
                Movement *= (MoveSpeed/3f);
                Rigidbody.velocity = new Vector3(
                    Movement.x,
                    Rigidbody.velocity.y,
                    Movement.z);
            }
            else 
            {
                Movement *= MoveSpeed;
                Rigidbody.velocity = new Vector3(
                    Movement.x,
                    Rigidbody.velocity.y,
                    Movement.z);
            }
        }

        public void ChangeToNormal()
        {
            IsInteracting = false;
            PlayerManager.ChangeToNormal();
        }
    }
}

