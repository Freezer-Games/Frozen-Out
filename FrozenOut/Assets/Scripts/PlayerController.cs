using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode { Stopped, Stealth, Normal, Running }

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public Animator playerAnim;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private const float STEALTH_SPEED = 6.5f;
    [SerializeField] private const float NORMAL_SPEED = 8f;
    [SerializeField] private const float RUN_SPEED = 10f;
    private Vector2 input;

    [Header("Camera values")]
    private Vector3 camForward = Vector3.zero;
    private Vector3 camRight = Vector3.zero;

    private Vector3 movement;

    [Header("Move state")]
    [SerializeField] private MoveMode moveState;

    void Start()
    {
        moveState = MoveMode.Stopped;
        moveSpeed = NORMAL_SPEED;
    }

    void Update()
    {   
        input = new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));
        input.Normalize();

        playerAnim.SetBool("walking", movement != Vector3.zero);

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
        }
        CameraVectors(); 
    }

    void FixedUpdate()
    {
        Move();

        if (Input.GetButton("Jump") && playerRb.velocity.y == 0f)
        {
            Jump();
        }
    }

    private void Move()
    {
        movement = input.x * camRight + input.y * camForward;
        movement.Normalize();

        playerRb.AddForce(movement * moveSpeed);
    }

    private void Jump()
    {
        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void MoveStateManage(MoveMode mode)
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

    private void FaceMovement()
    {
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
    }

    private void CameraVectors()
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
