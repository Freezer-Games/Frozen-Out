using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private Animator _animator;

    private CharacterController _characterController;

    public float Speed = 5.0f;

    public float RotationSpeed = 240.0f;

    private float Gravity = 20.0f;

    private Vector3 _moveDir = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        //_animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get Input for axis
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate the forward vector
        Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

        if (move.magnitude > 1f) move.Normalize();

        // Calculate the rotation for the player
        move = transform.InverseTransformDirection(move);

        // Get Euler angles
        float turnAmount = Mathf.Atan2(move.x, move.z);

        transform.Rotate(0, turnAmount *  RotationSpeed * Time.deltaTime, 0);

        if (_characterController.isGrounded)
        {
            //_animator.SetBool("run", move.magnitude> 0);

            _moveDir = transform.forward * move.magnitude;

            _moveDir *= Speed;

        }

        _moveDir.y -= Gravity * Time.deltaTime;

        _characterController.Move(_moveDir * Time.deltaTime);
    }
}
