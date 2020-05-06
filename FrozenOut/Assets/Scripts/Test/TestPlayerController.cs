using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum MoveMode { Stopped, Stealth, Normal, Running }

public class TestPlayerController : MonoBehaviour
{
    const float STEALTH_SPEED = 3.75f;
    const float NORMAL_SPEED = 4f;
    const float RUN_SPEED = 6.5f;

    Rigidbody Rigidbody;
    Animator Animator;
    Collider Collider;


    [Header("States")]
    [Space]
    [SerializeField] bool IsGrounded;
    [SerializeField] bool IsMoving => MoveState != MoveMode.Stopped; //TODO
    [SerializeField] bool IsFormChanged;
    [SerializeField] bool CanMove;
    [SerializeField] MoveMode MoveState;

        
    [Header("Movement")]
    [Space]
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpForce = 6f;
    public Transform Model;
        

    [Header("Jump Constraints")]
    [Space]
    [SerializeField] float CanJumpDist;
    public Transform RayOrigin;


    [Header("Interaction")]
    [Space]
    public LayerMask WhatIsGround;
    public GameObject ObstacleAtFront;


    [Header("Melting-Skill")]
    [Space]
    public GameObject Stick;
    public GameObject StickPoint;
    [SerializeField] bool HasStick;
    [SerializeField] bool OnStick;
    [SerializeField] bool IsRecovering;


    private Vector2 MoveInput;
    private Vector3 Movement;
    private Vector3 CamForward;
    private Vector3 CamRight;

    [Space]
    public UnityEvent MeltingEv;
    public UnityEvent RecoveryEv;
    
    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Collider = GetComponent<Collider>();
    }

    void Start()
    {
        MoveState = MoveMode.Stopped;
        MoveSpeed = NORMAL_SPEED;
        CanJumpDist = 0.51f;
        IsFormChanged = false;
        IsGrounded = true;
        HasStick = true;
        OnStick = false;
        CanMove = true;

        if (MeltingEv == null)
            MeltingEv = new UnityEvent();

        if (RecoveryEv == null)
            RecoveryEv = new UnityEvent();

    }

    void Update()
    {
        MoveInput = new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));
        MoveInput.Normalize();

        Animator.SetBool("isMoving", Movement != Vector3.zero);

        //Check there is an MoveInput
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

            if(Input.GetKeyDown(KeyCode.K)) 
            {
                if (!IsFormChanged && HasStick)
                {
                    Melting();
                }
                else if (IsFormChanged && OnStick)
                {
                    Recovery();
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
        if (CanMove)
        {
            if (!IsFormChanged && Input.GetButtonDown("Jump") && CheckGround())
                Jump();

            Move();
        }
        /*
        else
        {
            if (IsRecovering)
                MoveToTarget(StickPoint.transform, 0.1f, STEALTH_SPEED);
        }
        */
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
            ObstacleAtFront = other.gameObject;

        if (other.gameObject.CompareTag("Stick"))
            OnStick = true;
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
            ObstacleAtFront = null;

        if (other.gameObject.CompareTag("Stick"))
            OnStick = false;
    }

    void Move()
    {
        Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
        Movement.Normalize();
        Movement *= MoveSpeed;

        Rigidbody.velocity = new Vector3(Movement.x, Rigidbody.velocity.y, Movement.z);
    }

    void MoveToTarget(Transform target, float distanceToStop, float speed)
    {
        if (Vector3.Distance(transform.position, target.position) > distanceToStop)
        {
            transform.LookAt(target);
            Rigidbody.AddRelativeForce(Vector3.forward * speed, ForceMode.Force);
        }
        else
        {
            IsRecovering = false;
            Rigidbody.isKinematic = false;
            Collider.isTrigger = false;
        }
    }

    void Jump()
    {
        Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        IsGrounded = false;
        Animator.SetTrigger("isJumping");
    }

    bool CheckGround() {
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

    void Melting() 
    {
        Animator.SetTrigger("isChanging");
        IsFormChanged = true;
        HasStick = false;
        MeltingEv.Invoke();
    }

    void Recovery() 
    {
        Animator.SetTrigger("isChanging");
        IsRecovering = true;
        Rigidbody.isKinematic = true;
        Collider.isTrigger = true;
        
        CanMove = false;
        IsFormChanged = false;
        HasStick = true;
        RecoveryEv.Invoke();
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
        Model.rotation = 
            Quaternion.Slerp(Model.rotation, Quaternion.LookRotation(Movement.normalized), 0.2f);
    }

    void CameraVectors()
    {
        CamForward = Camera.main.transform.forward;
        CamRight = Camera.main.transform.right;
        CamForward.y = 0f;
        CamRight.y = 0f;
        CamForward.Normalize();
        CamRight.Normalize();
    }

    public MoveMode getMoveStatus()
    {
        return MoveState;
    }
}
