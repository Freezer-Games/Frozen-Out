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
        [SerializeField] float MoveSpeed = 4f;
        [SerializeField] float JumpForce = 6f;


        [Header("Jump")]
        float CanJumpDist = 0.51f;
        public Transform RayOrigin;

        [Header("Interact")]
        public Transform InteractItem;
        public Transform InteractPoint;

        public UnityEvent Melting;

        void Start()
        {
            CanMove = true;
            IsInteracting = false;
            Collider.enabled = true;

            if (Melting == null) 
                Melting = new UnityEvent();
        }

        void Update() 
        {
            if (PlayerManager.IsEnabled)
            {
                if (IsInteracting) 
                {
                    CanMove = false;
                    
                }
                else 
                {
                    CanMove = true;
                }

                if (CanMove)
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
            if (PlayerManager.IsEnabled)
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

        private void Jump() 
        {
            if (CheckGround())
            {
                Animator.SetTrigger("isJumping");
                Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }

        private bool CheckGround() 
        {
            RaycastHit hit;
            Ray checker = new Ray(RayOrigin.position, Vector3.down);
            Debug.DrawRay(RayOrigin.position, Vector3.down, Color.red, 0.1f);

            if (Physics.Raycast(checker, out hit, CanJumpDist)) 
            {
                if (hit.collider.CompareTag("Ground")) 
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
    }

}
