using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;
using Assets.Scripts.Dialogue;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Movement")]
    public float Speed = 7.5f;
    public float RotationSpeed = 240.0f;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 move;
    private Vector3 camForward_Dir;
    private float h, v;
    private bool moving;
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

    public event EventHandler<PlayerControllerEventArgs> Moving; 
    public event EventHandler Idle;
    private Animator animator;

    private GameObject snow;
    [System.NonSerialized]
    public bool canMove = true;

    private DialogueRunner dialogueSystemYarn;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        height = characterController.height;
        bendHeight = characterController.height - bendDiff;
        center = characterController.center;
        bendCenter = characterController.center;
        bendCenter.y -= (bendDiff / 2);
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();

        try { 
            snow = GameObject.Find("Snow");
            snow.transform.position = new Vector3(transform.position.x, snow.transform.position.y, transform.position.z);
        }
        catch {}
    }

    void Update()
    {
        if (dialogueSystemYarn.isDialogueRunning) { canMove = false; } else if (!dialogueSystemYarn.isDialogueWaiting) { canMove = true; }
        camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        MovementCalculation();
        
        if (characterController.isGrounded)
        {
            animator.SetBool("isMoving", moving);

            

            moveDir = transform.forward * move.magnitude;
            

            sneaking = false;

            if (Input.GetKey(GameManager.instance.crouch)) { //left control - va lento
                SneakyMode();
            }
            else {
                NormalMode();
            }
            moveDir.y = 0;

            if (Input.GetKeyDown(GameManager.instance.jump))
            {
                Jump();
            }
        }
        moveDir.y -= gravity * Time.deltaTime;
        StartCoroutine(MoveCoroutine(moving));
        characterController.Move(moveDir * Time.deltaTime);

        try {
            snow.transform.position = new Vector3(transform.position.x, snow.transform.position.y, transform.position.z);
        } catch {}
    }


    private void MovementCalculation() {
        if (canMove)
        {
            h = 0;
            v = 0;
            if (Input.GetKey(GameManager.instance.forward)) { v++; } else if (Input.GetKey(GameManager.instance.backward)) { v--; }
            //h = Input.GetAxis("Horizontal");
            if (Input.GetKey(GameManager.instance.left)) { h--; } else if (Input.GetKey(GameManager.instance.right)) { h++; }
            //v = Input.GetAxis("Vertical");

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

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);
        }
    }

    private void NormalMode() 
    {
        moveDir *= Speed;
        animator.SetTrigger("isSneakingOut");
        characterController.height = height;
        characterController.center = center;
    }

    IEnumerator MoveCoroutine(bool active)
    {
        if (active) yield return new WaitForSeconds(movementDelay);
        yield return null;
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