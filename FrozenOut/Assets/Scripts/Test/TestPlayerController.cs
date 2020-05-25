using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    Rigidbody Rigidbody;
    Animator Animator;

    [Header("States")]
    [Space]
    [SerializeField] bool IsGrounded;
    [SerializeField] bool IsFormChanged;
    [SerializeField] bool CanMove;


        
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
    public GameObject InteractiveObject;
    private Ore IntOre;
    [SerializeField] bool IsInteracting;
    [SerializeField] bool IsObjectAimed;


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
    }

    void Start()
    {
        MoveSpeed = 4f;
        CanJumpDist = 0.51f;
        IsFormChanged = false;
        HasStick = true;
        OnStick = false;
        CanMove = true;

        InteractiveObject = null;
        IsObjectAimed = false;
        IsInteracting = false;

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

        Animator.SetBool("isMoving", Movement != Vector3.zero || IsRecovering);

        if (IsRecovering || IsInteracting) CanMove = false;
        if (InteractiveObject == null) IsObjectAimed = false;

        //Check there is an MoveInput
        if (CanMove)
        {
            //Jump
            if (!IsFormChanged && Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (!IsFormChanged && IsObjectAimed && Input.GetKeyDown(KeyCode.P)) 
            {
                Debug.Log("Voy a minar");
                Rigidbody.isKinematic = true;
                CanMove = false;
                IsInteracting = true;
            }

            if (Movement != Vector3.zero)
            {
                FaceMovement();
            }
            else
            {
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
            else if (IsInteracting)
            {
                MoveToTarget(IntOre.GetInteractPoint(), 0.01f, 3f);
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
        if (other.gameObject.CompareTag("Stick"))
            OnStick = true;
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Stick"))
            OnStick = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interact") && !IsObjectAimed)
        {
            IsObjectAimed = true;
            InteractiveObject = other.gameObject;
            IntOre = InteractiveObject.GetComponent<Ore>();
            if (IntOre == null) Debug.Log("Soy nulo");
            else Debug.Log("No soy nulo");
            Debug.Log("Aimed: " + InteractiveObject.name); 
        }     
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stick"))
        {
            HasStick = false;
        }

        if (InteractiveObject == other.gameObject && !IsInteracting)
        {
            InteractiveObject = null;
            IsObjectAimed = false;
            IntOre = null;
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
        //Vector3 destino = new Vector3(target.position.x, transform.position.y, target.position.z);

        if (Vector3.Distance(target.position, transform.position) > distanceToStop)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

            if (IsRecovering) {
                transform.LookAt(target, Vector3.up);
            }  
        }
        else
        {
            if (IsRecovering)
            {
                RecoveryEv.Invoke();

                IsRecovering = false;
                IsFormChanged = false;
                HasStick = true;

                Rigidbody.isKinematic = false;
                Animator.SetTrigger("isChanging");
            }
            else if (IsInteracting)
            {
                Vector3 lookPos = new Vector3(InteractiveObject.transform.position.x,
                                                transform.position.y,
                                                InteractiveObject.transform.position.z);

                transform.LookAt(lookPos);

                Animator.SetTrigger("isMining");
                IntOre.Execute();
                Rigidbody.isKinematic = false;

                IsInteracting = false;
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

    void Melting() 
    {
        Animator.SetTrigger("isChanging");

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
}
