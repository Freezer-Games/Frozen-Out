using System;
using System.Linq;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Sound;

namespace Scripts.Level.Player
{
    public class PlayerController : MonoBehaviour
    {

        public PlayerManager PlayerManager;

        public ParticleSystem Dust;

        [Header("Movement")]
        public float Speed;
        private const float MAX_SPEED = 7.5f;
        private const float MIN_SPEED = 0.5f;
        public float RotationSpeed = 240.0f;

        private Vector3 MoveDirection = Vector3.zero;
        private Vector3 Move;
        private Vector3 CamForwardDirection;
        private float Horizontal;
        private float Vertical;
        private bool IsMoving => Move.magnitude > 0;
        public int CloseRadius;

        [Header("Jump")]
        public float JumpForce = 10.0f;
        private readonly float Gravity = 20.0f;
        

        [Header("Bending")]
        public float BendSpeed = 2.5f;
        private float Height, BendHeight;
        private Vector3 Center, BendCenter;
        private float BendJumpForce => JumpForce * 0.3f;
        private readonly float BendDiff = 0.4f;
        private bool IsSneaking;

        public bool IsGrounded => CharacterController.isGrounded;

        public event EventHandler<PlayerControllerEventArgs> Moving; 
        public event EventHandler Idle;
        private CharacterController CharacterController;
        private Animator Animator;

        private GameObject Snow;

        void Start()
        {
            Animator = GetComponent<Animator>();
            CharacterController = GetComponent<CharacterController>();

            Height = CharacterController.height;
            BendHeight = CharacterController.height - BendDiff;
            Center = CharacterController.center;
            BendCenter = CharacterController.center;
            BendCenter.y -= (BendDiff / 2);

            Snow = GameObject.Find("Snow");
            if (Snow != null)
            {
                Snow.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            }
        }

        void Update()
        {
            CamForwardDirection = Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // Reproduce o para el sonido de pisadas
            CheckAudio();

            // Comprueba si hay algun input
            //if (CheckInput()) 
            //{
            //    if (Speed > MAX_SPEED) Speed = MAX_SPEED;
            //    Speed += 0.5f;
            //}
            //else Speed = MIN_SPEED;

            // Avisa de que se va a mover
            PlayerControllerEventArgs controllerEvent = OnMoving();

            // Si alguien le ha dicho que cancele el movimiento, para
            if (controllerEvent.Cancel)
            {
                MoveDirection = Vector3.zero;
                IsSneaking = false;
                CheckSizeAndSpeed();
                OnIdle();
            }
            else
            {
                MovementCalculation();
                RotatePlayer();

                if (IsGrounded)
                {
                    Animator.SetBool("isMoving", IsMoving);

                    MoveDirection = transform.forward * Move.magnitude;

                    CheckSneaking();
                    CheckSizeAndSpeed();

                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        if (Dust.isPlaying) StopDust();
                        else CreateDust();
                    }

                    CheckJump();
                }
            }

            MoveDirection.y -= Gravity * Time.deltaTime;

            CharacterController.Move(MoveDirection * Time.deltaTime);

            if (Snow != null)
            {
                Snow.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            }
        }

        // Reproduce o para el sonido de pisadas según si se está moviendo o no
        private void CheckAudio()
        {
            if (PlayerManager.IsStepsPlaying())
            {
                if (!IsMoving)
                {
                    PlayerManager.StopSteps();
                }
            }
            else
            {
                if (IsMoving)
                {
                    PlayerManager.PlaySteps();
                }
            }
        }

        private void CheckSneaking()
        {
            if (Input.GetKey(PlayerManager.GetCrouchKey())) //left control - va lento
            {
                IsSneaking = true;
                Animator.SetTrigger("isSneakingIn");
            }
            else if (IsSneaking)
            {
                Animator.SetTrigger("isSneakingOut");
                IsSneaking = false;
            }
        }

        private void CheckSizeAndSpeed()
        {
            if (IsSneaking)
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
            MoveDirection.y = 0;

            if (Input.GetKeyDown(PlayerManager.GetJumpKey()))
            {
                Jump();
            }
        }

        private bool CheckInput() => PlayerManager.GetMovementKeys().Any(Input.GetKey);

        private void MovementCalculation()
        {
            Horizontal = 0;
            Vertical = 0;
            if (Input.GetKey(PlayerManager.GetForwardKey()))
            {
                Vertical++;
            }
            else if (Input.GetKey(PlayerManager.GetBackKey()))
            {
                Vertical--;
            }
            
            if (Input.GetKey(PlayerManager.GetLeftKey()))
            {
                Horizontal--;
            }
            else if (Input.GetKey(PlayerManager.GetRightKey()))
            {
                Horizontal++;
            }

            Move = Vertical * CamForwardDirection + Horizontal * UnityEngine.Camera.main.transform.right;

            if (Move.magnitude > 1f)
            {
                Move.Normalize();
            }
        }

        private void RotatePlayer()
        {
            // Calculate the rotation for the player
            Move = transform.InverseTransformDirection(Move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(Move.x, Move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);
        }

        private void NormalMode() 
        {
            MoveDirection *= Speed;
            CharacterController.height = Height;
            CharacterController.center = Center;
        }

        private void SneakyMode() 
        {
            IsSneaking = true;
            MoveDirection *= BendSpeed;
            CharacterController.height = BendHeight;
            CharacterController.center = BendCenter;
        }

        private void Jump() 
        {
            MoveDirection.y = IsSneaking? BendJumpForce : JumpForce;
            Animator.SetTrigger("isJumping");
        }

        protected virtual PlayerControllerEventArgs OnMoving()
        {
            
            PlayerControllerEventArgs controllerEvent = new PlayerControllerEventArgs();
            Moving?.Invoke(this, controllerEvent);
            return controllerEvent;
        }

        protected virtual void OnIdle()
        {
            Animator.SetBool("isMoving", false);
            Idle?.Invoke(this, EventArgs.Empty);
        }

        private void CreateDust() => Dust.Play();

        private void StopDust() => Dust.Stop();
    }

    [Serializable]
    public class PlayerControllerEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}