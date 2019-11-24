using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Movement")]
    public float Speed = 5.0f;
    public float RotationSpeed = 240.0f;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 move;
    private Vector3 camForward_Dir;
    private float h, v;
    private bool moving;


    [Header("Jump")]
    public float JumpForce = 10.0f;
    private readonly float gravity = 20.0f;
    

    [Header("Bending")]
    public float bendSpeed = 2.5f;
    private float height, bendHeight;
    private Vector3 center, bendCenter;
    private float bendJumpForce => JumpForce * 0.3f;
    private readonly float bendDiff = 0.4f;
    private bool sneaking;

    public event EventHandler<PlayerControllerEventArgs> Moving; 
    public event EventHandler Idle;
    private Animator animator;
    // Use this for initialization

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        height = characterController.height;
        bendHeight = characterController.height - bendDiff;
        center = characterController.center;
        bendCenter = characterController.center;
        bendCenter.y -= (bendDiff / 2);
    }

    // Update is called once per frame
    void Update()
    {
        camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        MovementCalculation();
        
        if (characterController.isGrounded)
        {
            animator.SetBool("isMoving", moving);
            moveDir = transform.forward * move.magnitude;

            sneaking = false;

            if (Input.GetButton("Fire1")) { //left control - va lento
                SneakyMode();
            }
            else {
                NormalMode();
            }
            moveDir.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        moveDir.y -= gravity * Time.deltaTime;
        characterController.Move(moveDir * Time.deltaTime);
    }

    private void MovementCalculation() {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        move = v * camForward_Dir + h * Camera.main.transform.right;

        if (move.magnitude > 1f) move.Normalize();

        // Calculate the rotation for the player
        move = transform.InverseTransformDirection(move);

        // Get Euler angles
        float turnAmount = Mathf.Atan2(move.x, move.z);

        moving = move.magnitude > 0;

        if (moving)
        {
            // Avisa de que se va a mover
            PlayerControllerEventArgs e = OnMoving();

            // Si alguien le ha dicho que cancele el movimiento, para
            if (e.Cancel)
            {
                moveDir = Vector3.zero;
                OnIdle();
                return;
            }
        } 
        else
        {
            OnIdle();
        }

        transform.Rotate(0, turnAmount *  RotationSpeed * Time.deltaTime, 0);
    }

    private void NormalMode() 
    {
        moveDir *= Speed;
        animator.SetTrigger("isSneakingOut");
        characterController.height = height;
        characterController.center = center;
    }

    private void SneakyMode() 
    {
        sneaking = true;
        moveDir *= bendSpeed;
        characterController.height = bendHeight;
        characterController.center = bendCenter;
        animator.SetTrigger("isSneakingIn");
    }

    private void Jump() 
    {
        moveDir.y = sneaking ? bendJumpForce : JumpForce;
        animator.SetTrigger("isJumping");
    }

    protected virtual PlayerControllerEventArgs OnMoving()
    {
        PlayerControllerEventArgs e = new PlayerControllerEventArgs();
        Moving?.Invoke(this, e);
        return e;
    }

    protected virtual void OnIdle()
    {
        animator.SetBool("isMoving", false);
        Idle?.Invoke(this, EventArgs.Empty);
    }
}

public class PlayerControllerEventArgs : EventArgs
{
    public bool Cancel { get; set; }
}