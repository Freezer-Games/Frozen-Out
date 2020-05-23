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
        [SerializeField] bool IsMoving;


        [Header("Movement")]
        [SerializeField] float MoveSpeed = 4f;
        [SerializeField] float MovementDelay;


        [Header("Interact")]
        public Transform InteractItem;
        public Transform InteractPoint;

        public UnityEvent Recovery;

        void Start() 
        {
            CanMove = true;
            IsInteracting = false;
            IsMoving = false;
            Collider.enabled = true;

            if (Recovery == null) 
                Recovery = new UnityEvent();
        }

        void Update()
        {
            if (PlayerManager.IsEnabled)
            {
                if (IsInteracting) 
                {
                    CanMove = false;
                    MoveToTarget(InteractPoint, 0.01f, 3f);
                }

                if (CanMove)
                {
                    MoveInput = new Vector2(
                        Input.GetAxis("Horizontal"),
                        Input.GetAxis("Vertical"));
                    MoveInput.Normalize();

                    Animator.SetBool("isMoving", MoveInput != Vector2.zero);

                    if (MoveInput != Vector2.zero)
                    {
                        FaceMovement();

                        if (!IsMoving) 
                        {
                            IsMoving = true;
                            StartCoroutine(SlimeMovement());
                            Debug.Log("Coroutine start");    
                        }
                    }
                    else
                    {
                        if (IsMoving)
                        {
                            IsMoving = false;
                            StopCoroutine(SlimeMovement());
                        }
                        
                        /*
                        if (Input.GetKeyDown(KeyCode.V))
                        {
                            PlayerManager.ChangeToNormal();
                            Recovery.Invoke();
                        }
                        */
                    }
                }
            }
        }
        void LateUpdate() 
        {
            CameraVectors();
        }

        void OnDisablae() 
        {
            Debug.Log("Desactivnado collider");
            Collider.enabled = false;
        }

        protected override void Move()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
            Movement *= MoveSpeed;

            Rigidbody.AddForce(Movement, ForceMode.Impulse);
        }

        public void ChangeToNormal()
        {
            IsInteracting = false;
            PlayerManager.ChangeToNormal();
        }

        IEnumerator SlimeMovement()
        {
            while(IsMoving)
            {
                Move();
                yield return new WaitForSeconds(MovementDelay);
            }

            Debug.Log("Corotine stoped");
            yield return null;
        }
    }
}

