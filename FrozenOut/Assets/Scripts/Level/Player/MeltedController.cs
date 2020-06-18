using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Player
{
    public class MeltedController : BasePlayerController
    {
        [Header("States")]
        public bool IsInteracting;
        [SerializeField] bool CanMove;
        [SerializeField] bool IsCharging;


        [Header("Movement")]
        [SerializeField] float MoveSpeed = 4f;


        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;
        public UnityEvent Recovery;


        void Start() 
        {
            Collider.center = new Vector3(0f, 0.25f, 0f);
            Collider.radius = 0.25f;
            Collider.height = 0.3f;

            CanMove = true;
            IsInteracting = false;
            InDeathZone = false;

            if (Recovery == null) 
                Recovery = new UnityEvent();
        }

        void Update()
        {
            if (PlayerManager.IsEnabled())
            {
                if (IsInteracting) 
                {
                    CanMove = false;
                    Rigidbody.isKinematic = true;
                    MoveToTarget(InteractPos, InteractLook, 0.01f, 0.5f);
                }
                else
                {
                    CanMove = true;
                    Rigidbody.isKinematic = false;
                }

                if (CanMove)
                {
                    MoveInput = new Vector2(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));
                    MoveInput.Normalize();

                    Animator.SetBool("isMoving", Movement.x != 0f && Movement.z != 0f);

                    if (Movement.x != 0f && Movement.z != 0f)
                    {
                        FaceMovement();
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (PlayerManager.IsEnabled())
            {
                if (CanMove)
                {
                    CalculeMove();
                }
            }
        }

        void LateUpdate() 
        {
            CameraVectors();
        }

        protected override void CalculeMove()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
              
            if (IsCharging) 
            {
                Movement *= (MoveSpeed / 3f);
                Rigidbody.velocity = new Vector3(
                    Movement.x,
                    Rigidbody.velocity.y,
                    Movement.z);
            }
            else 
            {
                Movement *= MoveSpeed;
                Rigidbody.velocity = new Vector3(
                    Movement.x,
                    Rigidbody.velocity.y,
                    Movement.z);
            }
        }

        public void ChangeToNormal()
        {
            IsInteracting = false;
            PlayerManager.ChangeToNormal();
        }

        void OnCollisionEnter(Collision other)
        {
            if (WhatIsGround == (WhatIsGround | (1 << other.gameObject.layer)))
            {
                if (InDeathZone)
                {
                    Debug.Log("me paro");
                    StopCoroutine(CountdownToDeath());
                    InDeathZone = false;
                }
            }

            if (DeathZone == (DeathZone | (1 << other.gameObject.layer)))
            {
                if (InDeathZone)
                {
                    StartCoroutine(CountdownToDeath());
                }
            }
        }
    }
}

