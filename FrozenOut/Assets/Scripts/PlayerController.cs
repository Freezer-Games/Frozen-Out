using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode { Stopped, Stealth, Normal, Running }

public class PlayerController : MonoBehaviour
{
    const float STEALTH_SPEED = 6.5f;
    const float NORMAL_SPEED = 8f;
    const float RUN_SPEED = 10f;

    [Header("Features")]
    Rigidbody playerRb;
    Animator playerAnim;
    [SerializeField] bool formChanged;
    [SerializeField] bool isGrounded;
    [SerializeField] private MoveMode moveState;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 5f;

    Vector2 input;
    Vector3 movement, camForward, camRight;

    public LayerMask whatIsGround;
    
    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    void Start()
    {
        moveState = MoveMode.Stopped;
        moveSpeed = NORMAL_SPEED;
        formChanged = false;
    }

    void Update()
    {
        input = new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));
        input.Normalize();

        //playerAnim.SetBool("walking", movement != Vector3.zero);

        //Check there is an input
        if (movement != Vector3.zero)
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
                formChanged = !formChanged;
                playerAnim.SetBool("changeForm", formChanged);
            }
        }

        CameraVectors();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            Jump();
        }

        Move();
    }

    void OnCollisionEnter(Collision other)
    {
        if (whatIsGround == (whatIsGround | (1 << other.gameObject.layer)))
        {
            isGrounded = true;
        }
    }

    void Move()
    {
        movement = input.x * camRight + input.y * camForward;
        movement.Normalize();

        playerRb.AddForce(movement * moveSpeed);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void MoveStateManage(MoveMode mode)
    {
        if (mode != moveState)
        {
            if (moveState == MoveMode.Stopped && mode == MoveMode.Normal)
            {
                moveState = mode;
                moveSpeed = NORMAL_SPEED;
            }
            else if (moveState == MoveMode.Normal && mode == MoveMode.Stopped)
            {
                moveState = mode;
            }
            else if (moveState == MoveMode.Normal && mode == MoveMode.Running)
            {
                moveState = mode;
                moveSpeed = RUN_SPEED;
            }
            else if (moveState == MoveMode.Running && mode == MoveMode.Normal)
            {
                moveState = mode;
                moveSpeed = NORMAL_SPEED;
            }
            else if (moveState == MoveMode.Normal && mode == MoveMode.Stealth)
            {
                moveState = mode;
                moveSpeed = STEALTH_SPEED;
            }
            else if(moveState == MoveMode.Stealth && mode == MoveMode.Normal)
            {
                moveState = mode;
                moveSpeed = NORMAL_SPEED;
            }
        }
    }

    void FaceMovement()
    {
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
    }

    void CameraVectors()
    {
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
    }

    public MoveMode getMoveStatus()
    {
        return moveState;
    }
}
