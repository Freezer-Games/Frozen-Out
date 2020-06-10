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
        [SerializeField] bool CanJump;


        [Header("Movement")]
        [SerializeField] Transform MainParent;
        [SerializeField] float MoveSpeed = 4f;
        [SerializeField] float JumpForce = 6f;


        [Header("Jump")]
        public float CanJumpDist = 0.51f;
        public Transform RayOrigin;
        public LayerMask WhatIsGround;

        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;

        public UnityEvent Melting;

        void Start()
        {
            CanMove = true;
            CanJump = false;
            IsInteracting = false;
            Collider.enabled = true;

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

                    if (Input.GetKeyDown(PlayerManager.GetJumpKey()) && CanJump) 
                    {
                        CanJump = false;
                        Jump();
                    }

                    if (Movement != Vector3.zero)
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
                    Move();
                }
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
            Movement *= MoveSpeed;

            Rigidbody.velocity = new Vector3(
                Movement.x,
                Rigidbody.velocity.y,
                Movement.z);
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("hola");
            if (other.CompareTag("Ascensor"))
            {
                Debug.Log("fuera");
                transform.SetParent(other.transform);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                CanJump = true;
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

        private void Jump() 
        {
            Rigidbody.isKinematic = false;
            Animator.SetTrigger("isJumping");
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        /*
        private bool CheckGround() 
        {
            RaycastHit hit;
            Ray checker = new Ray(RayOrigin.position, Vector3.down);
            Debug.DrawRay(RayOrigin.position, Vector3.down, Color.red, 0.1f);

            if (Physics.Raycast(checker, out hit, CanJumpDist, ~IgnoreByRay)) 
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) 
                {
                    Debug.Log("dist origin-hit: " + Vector3.Distance(hit.point, RayOrigin.position));
                    return true;
                }
                else 
                {
                    return false;
                }
            } 
            else 
            {
                return false;
            }
        
        }
        */
    }

}
