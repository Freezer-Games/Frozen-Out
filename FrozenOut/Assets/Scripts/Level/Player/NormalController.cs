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
        [SerializeField] bool CanMove;


        [Header("Movement")]
        [SerializeField] Transform MainParent;
        [SerializeField] float MoveSpeed;
        [SerializeField] float NormalSpeed = 8f;
        [SerializeField] float SneakingSpeed = 2.5f; 
        


        [Header("Jump")]
        [SerializeField] float JumpDistance = 8f;

        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;

        public UnityEvent Melting;


        void Start()
        {
            CharacterController.center = new Vector3(0f, 1f, 0f);
            CharacterController.radius = 0.5f;
            CharacterController.height = 2f;

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

                        Animator.SetBool("isMoving", Movement.x != 0f && Movement.z != 0f);

                        CalculeMove();

                        //Salto
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Jump();
                        }

                        //Agacharse
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            Animator.SetTrigger("IsSneakingIn");
                            MoveSpeed = SneakingSpeed;
                        }
                        
                        //Enderezarse
                        if (Input.GetKeyUp(KeyCode.LeftShift))
                        {
                            Animator.SetTrigger("IsSneakingOut");
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
            //Movement = transform.TransformDirection(Movement);
            Movement.x *= MoveSpeed;
            Movement.z *= MoveSpeed;
        }

        private void Jump()
        {
            Animator.SetTrigger("isJumping");
            Movement.y = JumpDistance;
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("hola");
            if (other.CompareTag("Ascensor"))
            {
                Debug.Log("fuera");
                transform.SetParent(other.transform);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ascensor"))
            {
                Debug.Log("dentro");
                transform.SetParent(MainParent);
            }
        }
    }

}
