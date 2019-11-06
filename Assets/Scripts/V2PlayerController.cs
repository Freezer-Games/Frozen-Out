using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2PlayerController : MonoBehaviour
{
    public float Speed = 100f;
    public float turnSpeed = 10f;

    Vector3 input;
    float angle;

    Quaternion targetRotation;
    Transform cam;

    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Transform groundChecker;

    void Start() {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(3); //la esfera
    }
    void Update() {
        GetInput();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.z) < 1) return;

        CalculateDirection();
        Rotate();   

        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }    

    }

    void FixedUpdate() 
    {
        Move();
    }


    void GetInput() {
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");
    }

    void CalculateDirection() {
        angle = Mathf.Atan2(input.x, input.z);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.z;
    }

    void Rotate() {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void Move() {
        rb.velocity = new Vector3(input.x * Speed, rb.velocity.y, input.z * Speed) * Time.deltaTime;
    }
}
