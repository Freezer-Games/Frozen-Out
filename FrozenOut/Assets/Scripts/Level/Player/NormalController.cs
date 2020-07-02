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
        public bool CanMove;
        [SerializeField] bool Grounded;
        Coroutine deathCoroutine;

        public bool inStealth;
      

        [Header("Movement")]
        [SerializeField] Transform MainParent;
        [SerializeField] float MoveSpeed;
        [SerializeField] float NormalSpeed = 4f;
        [SerializeField] float SneakingSpeed = 2.5f;


        [Header("Jump")]
        [SerializeField] float JumpForce = 6f;
        [SerializeField] LayerMask ignoredLayer;
        [SerializeField] float groundDistance;


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

            deathCoroutine = null;

            CanMove = true;
            IsInteracting = false;
            Grounded = false;
            InDeathZone = false;
            inStealth = false;
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
                    inStealth = false;
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

                    CheckWithRay();

                    if (Grounded)
                    {
                        if (Input.GetKey(PlayerManager.GetCrouchKey()))
                        {
                            Animator.SetTrigger("isSneakingIn");
                            inStealth = true;
                            MoveSpeed = SneakingSpeed;
                        }
                        else if(Input.GetKeyUp(PlayerManager.GetCrouchKey()) && inStealth)
                        {
                            Animator.SetTrigger("isSneakingOut");
                            inStealth = false;
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
                    Animator.SetTrigger("isSneakingOut");
                    Animator.SetBool("isMoving", false);
                }
            }
            else
            {
                CanMove = false;

                Animator.SetTrigger("isSneakingOut");
                Animator.SetBool("isMoving", false);
            }
        }

        void FixedUpdate()
        {
            if (PlayerManager.IsEnabled())
            {
                if (CanMove)
                {
                    CalculeMove();
                    CheckWithRay();

                    if (Grounded)
                    {
                        if (Input.GetKeyDown(PlayerManager.GetJumpKey()))
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

        void Jump()
        {
            Grounded = false;
            Animator.SetTrigger("isJumping");
            Animator.SetBool("isGrounded", false);
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpForce, Rigidbody.velocity.z);
        }

        void CheckWithRay()
        {
            Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.red, 1f);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundDistance, ~ignoredLayer))
            {
                if (WhatIsGround == (WhatIsGround | (1 << hit.transform.gameObject.layer)))
                {
                    Grounded = true;
                    Animator.SetBool("isGrounded", true);
                    
                    if (InDeathZone)
                    {
                        StopCoroutine(deathCoroutine);
                        InDeathZone = false;
                    }
                }

                if (DeathZone == (DeathZone | (1 << hit.transform.gameObject.layer)))
                {
                    if (!InDeathZone)
                    {
                        deathCoroutine = StartCoroutine(CountdownToDeath());
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ascensor"))
            {
                Debug.Log("en el ascensor");
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
