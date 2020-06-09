using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public PlayerManager PlayerManager;

        Rigidbody Rigidbody;
        Animator Animator;


        [Header("States")]
        [Space]
        public bool IsGrounded;
        [SerializeField] bool IsFormChanged;
        public bool IsMoving;

        
        [Header("Movement")]
        [Space]
        [SerializeField] float MoveSpeed = 4f;
        [SerializeField] float JumpForce = 6f;
        

        [Header("Jump Constraints")]
        [Space]
        [SerializeField] float CanJumpDist;
        public Transform RayOrigin;


        [Header("Interaction")]
        [Space]
        public Transform InteractItem;
        public Transform InteractPoint;
        public bool HasPickaxe; //Consultar a PlayerManager, que lo cogerá de Inventory

        [Header("Melting Skill")]
        [Space]
        public GameObject Stick;

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
            CanJumpDist = 0.51f;
            IsFormChanged = false;
            IsGrounded = false;
        }

        void Update()
        {
            if(PlayerManager.IsEnabled())
            {
                MoveInput = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")
                );
                MoveInput.Normalize();

                Animator.SetBool("isMoving", Movement != Vector3.zero);

                if (Input.GetKey(PlayerManager.GetJumpKey()) && !IsFormChanged)
                {
                    Jump();
                }

                //Check there is an input
                if (Movement != Vector3.zero)
                {
                    IsMoving = true;
                    FaceMovement();
                }
                else
                {
                    IsMoving = false;
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
            if(PlayerManager.IsEnabled())
            {
                Move();
            }
        }

        void OnCollisionEnter(Collision other)
        {

        }

        void OnCollisionExit(Collision other)
        {
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
            if (CheckGround()) 
            {
                Animator.SetTrigger("isJumping");
                Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
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

        public Animator GetAnimator()
        {
            return Animator;
        }
    }
}
