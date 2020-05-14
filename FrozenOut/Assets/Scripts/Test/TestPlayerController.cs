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
    CapsuleCollider CapsuleCol;
    BoxCollider BoxCol;

    public Transform distanceObj;
    public Transform distOrig;


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
    public Transform StickPoint;
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
        Animator = GetComponent<Animator>();
        CapsuleCol = GetComponent<CapsuleCollider>();
        BoxCol = GetComponent<BoxCollider>();
    }

    void Start()
    {
        MoveState = MoveMode.Stopped;
        MoveSpeed = NORMAL_SPEED;
        CanJumpDist = 0.51f;
        IsFormChanged = false;
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
        if (Input.GetKey(KeyCode.T))
            DistanceBtwObject();

        MoveInput = new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));
        MoveInput.Normalize();

        Animator.SetBool("isMoving", Movement != Vector3.zero || IsRecovering);

        if (IsRecovering) CanMove = false;

        //Check there is an MoveInput
        if (CanMove)
        {
            //Jump
            if (!IsFormChanged && Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            //Picking
            if (Input.GetKeyDown(KeyCode.P) && !IsFormChanged)
            {
                Movement = Vector3.zero;
                Animator.SetTrigger("isMining");
            }

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

                //Melting managemnet
                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (!IsFormChanged && HasStick)
                    {
                        Melting();
                    }
                    else if (IsFormChanged && OnStick)
                    {
                        Recovery();
                    }
                    else if (IsFormChanged && HasStick)
                    {
                        StaticRecovery();
                    }
                }
            }
        }
        else
        {
            if (IsRecovering)
            {
                MoveToTarget(StickPoint, 0.01f, 3f);
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
            Move();
        }
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

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stick"))
        {
            HasStick = false;
        }
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
        if (Vector3.Distance(target.position, transform.position) > distanceToStop)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
            transform.LookAt(target, Vector3.up);
            Debug.Log("me muevo hacia el punto");
        }
        else
        {
            if (IsRecovering)
            {
                BoxCol.enabled = false;
                CapsuleCol.enabled = true;

                RecoveryEv.Invoke();

                IsRecovering = false;
                IsFormChanged = false;
                HasStick = true;

                Rigidbody.isKinematic = false;
                Animator.SetTrigger("isChanging");
            }
        }
    }

    void Jump()
    {
        if (CheckGround())
        {
            Animator.SetTrigger("isJumping");
            Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
        
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

    void DistanceBtwObject()
    {
        if (distanceObj != null)
        {
            float dist = Vector3.Distance(distOrig.position, distanceObj.position);
            Debug.Log("Distancia: " + dist);
            Debug.DrawLine(distOrig.position, distanceObj.position, Color.red);
        }
    }

    void Melting() 
    {
        Animator.SetTrigger("isChanging");

        CapsuleCol.enabled = false;
        BoxCol.enabled = true;

        IsFormChanged = true;
        MeltingEv.Invoke();
    }

    void Recovery() 
    {
        IsRecovering = true;
        CanMove = false;
        Rigidbody.isKinematic = true;
    }

    void StaticRecovery()
    {
        RecoveryEv.Invoke();
        IsFormChanged = false;
        CanMove = false;
        Animator.SetTrigger("isChanging");
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
