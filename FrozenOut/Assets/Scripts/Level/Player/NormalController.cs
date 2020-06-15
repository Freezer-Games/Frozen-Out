using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Player 
{
    public class NormalController : BasePlayerController
    {
        [Header("States")]
        public bool IsInteracting;
        public bool Grounded;
        [SerializeField] bool CanMove;
      

        [Header("Movement")]
        [SerializeField] Transform MainParent;
        [SerializeField] float MoveSpeed;
        [SerializeField] float NormalSpeed = 4f;
        [SerializeField] float SneakingSpeed = 2.5f;


        [Header("Jump")]
        [SerializeField] float JumpForce = 6f;
        float DistanceToGround;
        [SerializeField] LayerMask WhatIsGround;

        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;

        public UnityEvent Melting;


        void Start()
        {
            Collider.center = new Vector3(0f, 1f, 0f);
            Collider.radius = 0.5f;
            Collider.height = 2f;

            DistanceToGround = GetComponent<Collider>().bounds.extents.y;

            CanMove = true;
            IsInteracting = false;
            MoveSpeed = NormalSpeed;

            if (Melting == null) 
                Melting = new UnityEvent();
        }

        void Update() 
        {
            if (PlayerManager.IsEnabled())
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
                    Grounded = Physics.Raycast(transform.position, Vector3.down, DistanceToGround + 0.1f);

                    if (Grounded)
                    {
                        MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));
                        MoveInput.Normalize();

                        Animator.SetBool("isMoving", Movement != Vector3.zero);

                        if (Input.GetKeyDown(PlayerManager.GetJumpKey()))
                        {
                            Jump();
                        }

                        if (Input.GetKeyDown(PlayerManager.GetCrouchKey()))
                        {
                            Animator.SetTrigger("isSneakingIn");
                            MoveSpeed = SneakingSpeed;
                        }

                        if (Input.GetKeyUp(PlayerManager.GetCrouchKey()))
                        {
                            Animator.SetTrigger("isSneakingOut");
                            MoveSpeed = NormalSpeed;
                        }

                        if (!(Movement != Vector3.zero))
                        {
                            if (Input.GetKeyDown(KeyCode.V))
                            {
                                PlayerManager.ChangeToMelted();
                                Melting.Invoke();
                            }
                        }
                    }

                    if (Movement != Vector3.zero)
                    {
                        FaceMovement();
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (PlayerManager.IsEnabled())
            {
                if (CanMove)
                {
                    if (Grounded)
                    {
                        CalculeMove();
                    }
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
            Movement *= MoveSpeed;

            Vector3 velocity = Rigidbody.velocity;
            Vector3 velocityChange = (Movement - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -MoveSpeed, MoveSpeed);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -MoveSpeed, MoveSpeed);
            velocityChange.y = 0f;

            Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private void Jump()
        {
            Grounded = false;
            Animator.SetTrigger("isJumping");
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                Grounded = true;
            }
        }

        void OnCollsionExit(Collision other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                Grounded = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ascensor"))
            {
                transform.SetParent(other.transform);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ascensor"))
            {
                transform.SetParent(MainParent);
            }
        }
    }
}
