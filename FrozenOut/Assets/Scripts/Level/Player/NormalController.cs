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
        [SerializeField] bool Grounded;
        [SerializeField] bool CanMove;
      

        [Header("Movement")]
        [SerializeField] Transform MainParent;
        [SerializeField] float MoveSpeed;
        [SerializeField] float NormalSpeed = 4f;
        [SerializeField] float SneakingSpeed = 2.5f;


        [Header("Jump")]
        [SerializeField] float JumpForce = 6f;


        [Header("Particles")]
        public ParticleSystem MeltingPart;
        ParticleSystem.EmissionModule PartEmiter;


        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;


        public UnityEvent Melting;


        void Start()
        {
            Collider.center = new Vector3(0f, 1f, 0f);
            Collider.radius = 0.5f;
            Collider.height = 2f;

            CanMove = true;
            IsInteracting = false;
            Grounded = false;
            InDeathZone = false;
            MoveSpeed = NormalSpeed;

            PartEmiter = MeltingPart.emission;

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
                    MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));
                    MoveInput.Normalize();

                    Animator.SetBool("isMoving", Movement != Vector3.zero);

                    if (Grounded)
                    {
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
                else
                {
                    Animator.SetBool("isMoving", false);
                }
            }
        }

        void FixedUpdate()
        {
            if (PlayerManager.IsEnabled())
            {
                if (CanMove)
                {
                    CalculeMove();

                    if (Grounded)
                    {
                        if (Input.GetKey(PlayerManager.GetJumpKey()))
                        {
                            Jump();    
                        }
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
            Animator.SetTrigger("isJumping");
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        void OnCollisionEnter(Collision other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                Grounded = true;

                if (InDeathZone)
                {
                    Debug.Log("me paro");
                    StopCoroutine(CountdownToDeath());
                    PartEmiter.enabled = false;
                    InDeathZone = false;
                }
            }

            if (DeathZone == (DeathZone | (1 << other.gameObject.layer)))
            {
                if (!InDeathZone)
                {
                    InDeathZone = true;
                    StartCoroutine(CountdownToDeath());
                    PartEmiter.enabled = true;
                }
            }
        }

        void OnCollisionExit(Collision other)
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
