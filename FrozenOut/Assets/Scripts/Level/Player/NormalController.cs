using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Player 
{
    public class NormalController : MonoBehaviour
    {
        public PlayerBase PlayerBase;
        public Rigidbody Rigidbody;
        public Animator Animator;

        [Header("States")]
        public bool inStealth;
      
        [Header("Movement")]
        [SerializeField] Transform MainParent;
        private Vector3 MovementDir;
        float MoveSpeed;
        [SerializeField] float NormalSpeed = 4f;
        [SerializeField] float SneakingSpeed = 2.5f;

        [Header("Jump")]
        [SerializeField] float JumpForce = 6f;

        [Header("Effects")]
        public GameObject MeltingPart;

        [Header("Interact")]
        public UnityEvent Melting;

        void Start()
        {
            PlayerBase.CanMove = true;
            PlayerBase.IsInteracting = false;
            PlayerBase.Grounded = false;
            inStealth = false;
            MoveSpeed = NormalSpeed;

            if (Melting == null) 
                Melting = new UnityEvent();
        }

        void Update() 
        {
            if (PlayerBase.PlayerManager.IsEnabled())
            { 
                if (PlayerBase.IsInteracting) 
                {
                    inStealth = false;
                }
   
                if (PlayerBase.CanMove)
                {
                    Animator.SetBool("isMoving", MovementDir != Vector3.zero);

                    if (PlayerBase.InDeathZone) MeltingPart.SetActive(true);
                    else MeltingPart.SetActive(false);

                    if (PlayerBase.Grounded)
                    {
                        if (Input.GetKey(PlayerBase.PlayerManager.GetCrouchKey()))
                        {
                            Animator.SetTrigger("isSneakingIn");
                            inStealth = true;
                            MoveSpeed = SneakingSpeed;
                        }
                        else if(Input.GetKeyUp(PlayerBase.PlayerManager.GetCrouchKey()) && inStealth)
                        {
                            Animator.SetTrigger("isSneakingOut");
                            inStealth = false;
                            MoveSpeed = NormalSpeed;
                        }

                        if (!(MovementDir != Vector3.zero))
                        {
                            /*
                            if (Input.GetKeyDown(KeyCode.V))
                            {
                                PlayerBase.PlayerManager.ChangeToMelted();
                                Melting.Invoke();
                            }
                            */
                        }
                    }

                    if (MovementDir != Vector3.zero)
                    {
                        PlayerBase.FaceMovement();
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (PlayerBase.PlayerManager.IsEnabled())
            {
                if (PlayerBase.CanMove)
                {
                    CalculeMove();

                    if (PlayerBase.Grounded)
                    {
                        if (Input.GetKeyDown(PlayerBase.PlayerManager.GetJumpKey()))
                        {
                            Jump();    
                        }
                    }
                }
            }
        }

        protected void CalculeMove() 
        {
            MovementDir = PlayerBase.GetMovement();
            MovementDir *= MoveSpeed;

            Vector3 velocity = PlayerBase.Rigidbody.velocity;
            Vector3 velocityChange = (MovementDir - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -MoveSpeed, MoveSpeed);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -MoveSpeed, MoveSpeed);
            velocityChange.y = 0f;

            Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        void Jump()
        {
            PlayerBase.Grounded = false;
            Animator.SetTrigger("isJumping");
            Animator.SetBool("isGrounded", false);
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpForce, Rigidbody.velocity.z);
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
