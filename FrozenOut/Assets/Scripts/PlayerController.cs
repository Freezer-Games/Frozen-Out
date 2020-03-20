using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    private Vector2 input;

    [Header("Camera")]
    private Vector3 camForward = Vector3.zero;
    private Vector3 camRight = Vector3.zero;

    private Vector3 movement;

    void Start()
    {
        speed = 10f;
    }

    void Update()
    {
        input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        input.Normalize();

        if (movement != Vector3.zero)
        {
            FaceMovement();
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

        playerRb.AddForce(movement * speed);
    }

    private void Jump()
    {
        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
}
