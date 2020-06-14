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

            AntiWall.SetActive(true);

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
                    MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));
                    MoveInput.Normalize();

                    Animator.SetBool("isMoving", Movement.x != 0f && Movement.z != 0f);

                    //Salto
                    if (Input.GetKeyDown(PlayerManager.GetJumpKey()))
                    {
                        Jump();
                    }

                    //Agacharse
                    if (Input.GetKeyDown(PlayerManager.GetCrouchKey()))
                    {
                        Animator.SetTrigger("isSneakingIn");
                        MoveSpeed = SneakingSpeed;
                    }
                        
                    //Enderezarse
                    if (Input.GetKeyUp(PlayerManager.GetCrouchKey()))
                    {
                        Animator.SetTrigger("isSneakingOut");
                        MoveSpeed = NormalSpeed;
                    }

                    if (Movement.x != 0f && Movement.z != 0f)
                    {
                        FaceMovement();
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.V))
                        {
                            PlayerManager.ChangeToMelted();
                            Melting.Invoke();
                        }
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
                    CalculeMove();
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

            Rigidbody.velocity = new Vector3(
                Movement.x,
                Rigidbody.velocity.y,
                Movement.z);
        }

        private void Jump()
        {
            if (Grounded)
            {
                Animator.SetTrigger("isJumping");
                Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                Grounded = true;
            }

            if (other.CompareTag("Ascensor"))
            {
                transform.SetParent(other.transform);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                Grounded = false;
            }

            if (other.CompareTag("Ascensor"))
            {
                transform.SetParent(MainParent);
            }
        }

        void OnCollisionStay(Collision other)
        {
            foreach (ContactPoint contact in other.contacts)
            {
                var colName = contact.thisCollider.name;

                if (colName == "Anti Wall" && !Grounded)
                {
                    Rigidbody.velocity = new Vector3(0f, -4f, 0);
                }
            }
        }
    }
}
