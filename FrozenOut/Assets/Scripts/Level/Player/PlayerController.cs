using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        const float STEALTH_SPEED = 3.75f;
        const float NORMAL_SPEED = 4f;
        const float RUN_SPEED = 6.5f;

        public PlayerManager PlayerManager;

        public bool IsGrounded;
        public bool IsMoving => MoveState != MoveMode.Stopped; //TODO

        [Header("Features")]
        Rigidbody Rigidbody;
        Animator Animator;
        [SerializeField]
        private bool IsFormChanged;
        [SerializeField]
        private MoveMode MoveState;

        [Header("Movement")]
        [SerializeField]
        private float MoveSpeed;
        [SerializeField]
        private float CurrentSpeed;
        [SerializeField]
        private float JumpForce = 3f;
        [SerializeField]
        private float Acceleration = 1.5f;

        public GameObject Pickaxe;
        public GameObject ObstacleAtFront;
        public bool HasPickaxe; //Consultar a PlayerManager, que lo cogerá de Inventory

        private Vector2 MoveInput;
        private Vector3 Movement;
        private Vector3 CamForward;
        private Vector3 CamRight;

        public LayerMask WhatIsGround;
        
        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            MoveState = MoveMode.Stopped;
            MoveSpeed = NORMAL_SPEED;
            CurrentSpeed = 0;
            IsFormChanged = false;
            IsGrounded = false;
        }

        void Update()
        {
            if(PlayerManager.IsEnabled)
            {
                MoveInput = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")
                );
                MoveInput.Normalize();

                Animator.SetBool("walking", Movement != Vector3.zero);

                //Check there is an input
                if (Movement != Vector3.zero)
                {
                    FaceMovement();
                    CurrentSpeed = Mathf.Lerp(CurrentSpeed, MoveSpeed, Acceleration);

                    if (Input.GetButton("Run"))
                    {
                        MoveStateManage(MoveMode.Running);
                    }
                    else if (Input.GetButton("Crouch"))
                    {
                        MoveStateManage(MoveMode.Stealth);
                    }
                    else
                    {
                        MoveStateManage(MoveMode.Normal);
                    }
                }
                else
                {
                    MoveStateManage(MoveMode.Stopped);

                    CurrentSpeed = Mathf.Lerp(CurrentSpeed, 0, Acceleration);

                    //Form change only if stopped
                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        IsFormChanged = !IsFormChanged;
                        Animator.SetBool("changeForm", IsFormChanged);
                    }
                }
            }
        }

        void LateUpdate()
        {
            CameraVectors();
        }

        void FixedUpdate()
        {
            if(PlayerManager.IsEnabled)
            {
                if (Input.GetKey(PlayerManager.GetJumpKey()) && IsGrounded)
                {
                    Jump();
                }
                Move();
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                ObstacleAtFront = other.gameObject;
            }
        }

        void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                ObstacleAtFront = null;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                IsGrounded = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                IsGrounded = false;
            }
        }

        void Move()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();

            Rigidbody.velocity = new Vector3(
                Movement.x * CurrentSpeed,
                Rigidbody.velocity.y,
                Movement.z * CurrentSpeed);
        }

        void Jump()
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        void MoveStateManage(MoveMode mode)
        {
            if (mode != MoveState)
            {
                if (MoveState == MoveMode.Stopped && mode == MoveMode.Normal)
                {
                    MoveState = mode;
                    MoveSpeed = NORMAL_SPEED;
                }
                else if (MoveState == MoveMode.Normal && mode == MoveMode.Stopped)
                {
                    MoveState = mode;
                }
                else if (MoveState == MoveMode.Normal && mode == MoveMode.Running)
                {
                    MoveState = mode;
                    MoveSpeed = RUN_SPEED;
                }
                else if (MoveState == MoveMode.Running && mode == MoveMode.Normal)
                {
                    MoveState = mode;
                    MoveSpeed = NORMAL_SPEED;
                }
                else if (MoveState == MoveMode.Normal && mode == MoveMode.Stealth)
                {
                    MoveState = mode;
                    MoveSpeed = STEALTH_SPEED;
                }
                else if(MoveState == MoveMode.Stealth && mode == MoveMode.Normal)
                {
                    MoveState = mode;
                    MoveSpeed = NORMAL_SPEED;
                }
            }
        }

        void FaceMovement()
        {
            transform.rotation = 
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Movement.normalized), 0.2f);
        }

        void CameraVectors()
        {
            CamForward = UnityEngine.Camera.main.transform.forward;
            CamRight = UnityEngine.Camera.main.transform.right;
            CamForward.y = 0f;
            CamRight.y = 0f;
            CamForward.Normalize();
            CamRight.Normalize();
        }

        public MoveMode GetMoveStatus()
        {
            return MoveState;
        }
    }

    public enum MoveMode { Stopped, Stealth, Normal, Running }
}
