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

        Rigidbody Rigidbody;
        Animator Animator;


        [Header("States")]
        [Space]
        public bool IsGrounded;
        public bool IsMoving => MoveState != MoveMode.Stopped; //TODO
        [SerializeField] bool IsFormChanged;
        [SerializeField] MoveMode MoveState;

        
        [Header("Movement")]
        [Space]
        [SerializeField] float MoveSpeed;
        [SerializeField] float JumpForce = 6f;
        

        [Header("Jump Constraints")]
        [Space]
        [SerializeField] float CanJumpDist;
        public Transform RayOrigin;
        public Transform Body;


        [Header("Interaction")]
        [Space]
        public LayerMask WhatIsGround;
        public GameObject Pickaxe;
        public GameObject ObstacleAtFront;
        public bool HasPickaxe; //Consultar a PlayerManager, que lo cogerá de Inventory

        private Vector2 MoveInput;
        private Vector3 Movement;
        private Vector3 CamForward;
        private Vector3 CamRight;   
        
        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            MoveState = MoveMode.Stopped;
            MoveSpeed = NORMAL_SPEED;
            CanJumpDist = 0.51f;
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

                //Animator.SetBool("walking", Movement != Vector3.zero);

                //Check there is an input
                if (Movement != Vector3.zero)
                {
                    FaceMovement();

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

                    //Form change only if stopped
                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        IsFormChanged = !IsFormChanged;
                        //Animator.SetBool("changeForm", IsFormChanged);
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
                if (Input.GetKey(PlayerManager.GetJumpKey()) && CheckGround())
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

        void Move()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
            Movement *= MoveSpeed;

            Rigidbody.velocity = new Vector3(
                Movement.x,
                Rigidbody.velocity.y,
                Movement.z);
        }

        void Jump()
        {
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            IsGrounded = false;
        }

        bool CheckGround() 
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
                    Debug.Log("aqui no se puede saltar");
                    return false;
                }
            } 
            else 
            {
                Debug.Log("en el aire");
                return false;
            }
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
            Body.rotation = 
                Quaternion.Slerp(Body.rotation, Quaternion.LookRotation(Movement.normalized), 0.2f);
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

        public Animator GetAnimator()
        {
            return Animator;
        }
    }

    public enum MoveMode { Stopped, Stealth, Normal, Running }
}
