using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class PlayerControllerEventArgs : EventArgs
{
    public bool Cancel { get; set; }
}

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Movement")]
    public float Speed = 7.5f;
    public float RotationSpeed = 240.0f;
    public float MOVEDELAY;
    private float delay;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 move;
    private Vector3 camForward_Dir;
    private float h, v;
    private bool moving => move.magnitude > 0;
    public int closeRadius;
    private const float movementDelay = 0.333f;

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

    public bool IsGrounded => characterController.isGrounded;

    public event EventHandler<PlayerControllerEventArgs> Moving; 
    public event EventHandler Idle;
    private Animator animator;
    public AudioSource steps;

    private GameObject snow;

    void Start()
    {
        steps = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        height = characterController.height;
        bendHeight = characterController.height - bendDiff;
        center = characterController.center;
        bendCenter = characterController.center;
        bendCenter.y -= (bendDiff / 2);
        delay = MOVEDELAY;

        snow = GameObject.Find("Snow");
        if (snow != null)
        {
            snow.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }
    }

    void Update()
    {
        camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // Reproduce o para el sonido de pisadas
        CheckAudio();

        // Compruba si hay algun input
        if (CheckInput()) delay -= Time.deltaTime;
        else delay = MOVEDELAY;

        // Avisa de que se va a mover
        PlayerControllerEventArgs e = OnMoving();

        // Si alguien le ha dicho que cancele el movimiento, para
        if (e.Cancel)
        {
            moveDir = Vector3.zero;
            sneaking = false;
            CheckSizeAndSpeed();
            OnIdle();
        }
        else
        {
            MovementCalculation();
            RotatePlayer();

            if (IsGrounded)
            {
                animator.SetBool("isMoving", moving);

                moveDir = transform.forward * move.magnitude;

                CheckSneaking();
                CheckSizeAndSpeed();

                CheckJump();
            }
        }

        moveDir.y -= gravity * Time.deltaTime;

        if (delay <= 0f) 
            characterController.Move(moveDir * Time.deltaTime);

        if (snow != null)
        {
            snow.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }
    }

    // Reproduce o para el sonido de pisadas según si se está moviendo o no
    private void CheckAudio()
    {
        if (steps.isPlaying)
        {
            if (!moving)
            {
                steps.Stop();
            }
        }
        else
        {
            if (moving)
            {
                steps.Play();
            }
        }
    }

    private void CheckSneaking()
    {
        if (Input.GetKey(GameManager.instance.crouch)) //left control - va lento
        {
            sneaking = true;
            animator.SetTrigger("isSneakingIn");
        }
        else if (sneaking)
        {
            animator.SetTrigger("isSneakingOut");
            sneaking = false;
        }
    }

    private void CheckSizeAndSpeed()
    {
        if (sneaking)
        {
            SneakyMode();
        }
        else
        {
            NormalMode();
        }
    }

    private void CheckJump()
    {
        moveDir.y = 0;

        if (Input.GetKeyDown(GameManager.instance.jump))
        {
            Jump();
        }
    }

    private bool CheckInput() {
        if (Input.GetKey(GameManager.instance.forward) ||
            Input.GetKey(GameManager.instance.backward) ||
            Input.GetKey(GameManager.instance.left) ||
            Input.GetKey(GameManager.instance.right))
            {
                return true;
            }
        
        return false;
    }

    private void MovementCalculation()
    {
        h = 0;
        v = 0;
        if (Input.GetKey(GameManager.instance.forward)) { v++; } else if (Input.GetKey(GameManager.instance.backward)) { v--; }
        //h = Input.GetAxis("Horizontal");
        if (Input.GetKey(GameManager.instance.left)) { h--; } else if (Input.GetKey(GameManager.instance.right)) { h++; }
        //v = Input.GetAxis("Vertical");

        move = v * camForward_Dir + h * Camera.main.transform.right;

        if (move.magnitude > 1f) move.Normalize();
    }

    private void RotatePlayer()
    {
        // Calculate the rotation for the player
        move = transform.InverseTransformDirection(move);

        // Get Euler angles
        float turnAmount = Mathf.Atan2(move.x, move.z);

        transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);
    }

    private void NormalMode() 
    {
        moveDir *= Speed;
        characterController.height = height;
        characterController.center = center;
    }

    private void SneakyMode() 
    {
        sneaking = true;
        moveDir *= bendSpeed;
        characterController.height = bendHeight;
        characterController.center = bendCenter;
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