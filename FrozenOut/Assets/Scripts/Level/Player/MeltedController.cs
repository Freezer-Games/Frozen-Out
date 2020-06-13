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


        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;

        public UnityEvent Recovery;


        void Start() 
        {
            CharacterController.center = new Vector3(0f, 0.25f, 0f);
            CharacterController.radius = 0.25f;
            CharacterController.height = 0.3f;

            CanMove = true;
            IsInteracting = false;
            IsMoving = false;

            if (Recovery == null) 
                Recovery = new UnityEvent();
        }

        void Update()
        {
            if (PlayerManager.IsEnabled())
            {
                if (IsInteracting) 
                {
                    CanMove = false;
                    MoveToTarget(InteractPos, InteractLook, 0.01f, 0.5f);
                }
                else
                {
                    CanMove = true;
                }

                if (CanMove)
                {
                    if (CharacterController.isGrounded)
                    {
                        MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));
                        MoveInput.Normalize();

                        CalculeMove();

                        Animator.SetBool("isMoving", Movement.x != 0f && Movement.z != 0f);

                        if (Movement.x != 0f && Movement.z != 0f)
                        {
                            FaceMovement();
                        }
                    }

                    Movement.y -= Gravity * Time.deltaTime;
                    CharacterController.Move(Movement * Time.deltaTime);
                }
            }
        }

        void LateUpdate() 
        {
            CameraVectors();
        }

        protected override void CalculeMove()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
            
            
            if (IsCharging) 
            {
                Movement.x *= (MoveSpeed/3f);
                Movement.z *= (MoveSpeed/3f);
            }
            else 
            {
                Movement.x *= MoveSpeed;
                Movement.z *= MoveSpeed;
            }
        }

        public void ChangeToNormal()
        {
            IsInteracting = false;
            PlayerManager.ChangeToNormal();
        }
    }
}

