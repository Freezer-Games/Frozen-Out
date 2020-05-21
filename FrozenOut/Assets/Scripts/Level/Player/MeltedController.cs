using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player
{
    public class MeltedController : BasePlayerController
    {
        [Header("States")]
        [SerializeField] bool IsInteracting;
        [SerializeField] bool CanMove;


        [Header("Movement")]
        [SerializeField] float MoveSpeed = 4f;
        [SerializeField] float MovementDelay;

        
        void Start() 
        {
            CanMove = true;
            IsInteracting = false;
        }

        void Update()
        {
            if (PlayerManager.IsEnabled)
            {
                if (CanMove)
                {
                    MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));

                    MoveInput.Normalize();

                    Animator.SetBool("isMoving", Movement != Vector3.zero);

                    if (Movement != Vector3.zero)
                    {
                        FaceMovement();
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.V))
                        {
                            PlayerManager.ChangeToNormal();
                        }
                    }
                }
            }
        }

        void FixedUpdate() 
        {
            if (CanMove) 
            {
                if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
                {
                    StartCoroutine(SlimeMovement());
                }
                else if (Input.GetButtonUp("Horizontal") && Input.GetButtonUp("Vertical"))
                {
                    StopCoroutine(SlimeMovement());
                }
            }
        }

        void LateUpdate() 
        {
            CameraVectors();
        }

        protected override void Move()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
            Movement *= MoveSpeed;

            Rigidbody.AddForce(Movement, ForceMode.Impulse);
        }

        IEnumerator SlimeMovement()
        {
            while(true)
            {
                Move();
                yield return new WaitForSeconds(MovementDelay);
            }
        }
    }
}

