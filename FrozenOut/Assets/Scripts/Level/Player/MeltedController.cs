using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Level.Player
{
    public class MeltedController : MonoBehaviour
    {
        public PlayerBase PlayerBase;
        public Rigidbody Rigidbody;
        public Animator Animator;

        [Header("States")]
        [SerializeField] bool IsCharging;

        [Header("Movement")]
        private Vector3 MovementDir;
        [SerializeField] float MoveSpeed = 4f;

        [Header("Interact")]
        public UnityEvent Recovery;

        void Start() 
        {
            PlayerBase.CanMove = true;
            PlayerBase.IsInteracting = false;

            if (Recovery == null) 
                Recovery = new UnityEvent();
        }

        void Update()
        {
            if (PlayerBase.PlayerManager.IsEnabled())
            {
                if (PlayerBase.CanMove)
                {
                    Animator.SetBool("isMoving", MovementDir != Vector3.zero);

                    if (MovementDir != Vector3.zero)
                    {
                        PlayerBase.FaceMovement();
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (PlayerBase.PlayerManager.IsEnabled())
            {
                if (PlayerBase.CanMove)
                {
                    if (PlayerBase.Grounded)
                    {
                        CalculeMove();
                    } 
                }
            }
        }

        protected void CalculeMove()
        {
            MovementDir = PlayerBase.GetMovement();
              
            if (IsCharging) 
            {
                MovementDir *= (MoveSpeed / 3f);
                Rigidbody.velocity = new Vector3(
                    MovementDir.x,
                    Rigidbody.velocity.y,
                    MovementDir.z);
            }
            else 
            {
                MovementDir *= MoveSpeed;
                Rigidbody.velocity = new Vector3(
                    MovementDir.x,
                    Rigidbody.velocity.y,
                    MovementDir.z);
            }
        }

        public void ChangeToNormal()
        {
            PlayerBase.IsInteracting = false;
            PlayerBase.PlayerManager.ChangeToNormal();
        }
    }
}

