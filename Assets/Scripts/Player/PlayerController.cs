﻿using System;
using System.Collections;
using UnityEngine;

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

    private GameObject snow;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        height = characterController.height;
        bendHeight = characterController.height - bendDiff;
        center = characterController.center;
        bendCenter = characterController.center;
        bendCenter.y -= (bendDiff / 2);

        snow = GameObject.Find("Snow");
        if (snow != null)
        {
            snow.transform.position = new Vector3(transform.position.x, snow.transform.position.y, transform.position.z);
        }

        Moving += (s, e) =>
        {
            if (Input.GetKey(KeyCode.P)) e.Cancel = true;
        };
    }

    void Update()
    {
        camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

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

        StartCoroutine(MoveCoroutine(moving));
        characterController.Move(moveDir * Time.deltaTime);

        if (snow != null)
        {
            snow.transform.position = new Vector3(transform.position.x, snow.transform.position.y, transform.position.z);
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

    IEnumerator MoveCoroutine(bool active)
    {
        if (active) yield return new WaitForSeconds(movementDelay);
        yield return null;
    }
}

public class PlayerControllerEventArgs : EventArgs
{
    public bool Cancel { get; set; }
}