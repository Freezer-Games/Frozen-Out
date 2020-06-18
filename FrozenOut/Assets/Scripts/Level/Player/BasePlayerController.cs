using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player 
{
    public abstract class BasePlayerController: MonoBehaviour
    {
        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        public PlayerManager PlayerManager;
        public Rigidbody Rigidbody;
        public Animator Animator;
        public CapsuleCollider Collider;

        [SerializeField] float CdToDeath;
        public LayerMask DeathZone;
        public LayerMask WhatIsGround;
        public bool InDeathZone;

        protected Vector2 MoveInput;
        protected Vector3 Movement;
        protected Vector3 CamForward;
        protected Vector3 CamRight;

        protected abstract void CalculeMove();

        public void MoveToTarget(Transform targetPos,Transform targetLook, float distanceToStop, float speed)
        {
            //Interactua y ya
            if (targetPos == null && targetLook == null) 
            {
                PlayerManager.SetIsInteracting(false);
                PlayerManager.SetInteractiveItem(null, null);
            }
            //Mira al objetivo e interactua
            else if (targetPos == null && targetLook != null)
            {
                Vector3 lookPos = targetLook.position;
                lookPos.y = transform.position.y;
                transform.LookAt(lookPos);

                PlayerManager.SetIsInteracting(false);
                PlayerManager.SetInteractiveItem(null, null);
            }
            //Se mueve a la posicion deseada minetras mira y luego interactua
            else 
            {
                Vector3 lookPos = targetLook.position;
                lookPos.y = transform.position.y;
                transform.LookAt(lookPos);

                Vector3 goPos = targetPos.position;
                goPos.y = 0f;

                if (Vector3.Distance(goPos, transform.position) > distanceToStop) 
                {     
                    Animator.SetBool("isMoving", true);

                    transform.position = 
                        Vector3.MoveTowards(transform.position, goPos, speed * Time.fixedDeltaTime);
                }
                else 
                {
                    Animator.SetBool("isMoving", false);           
                    PlayerManager.SetIsInteracting(false); 
                    PlayerManager.SetInteractiveItem(null, null);
                }
            }
        }

        public void FaceMovement() 
        {
            Vector3 direction = new Vector3(Movement.x, 0f, Movement.z);
            transform.rotation = 
                Quaternion.Slerp(
                    transform.rotation, 
                    Quaternion.LookRotation(direction.normalized), 
                    0.2f);
        }

        protected void CameraVectors() 
        {
            CamForward = UnityEngine.Camera.main.transform.forward;
            CamRight = UnityEngine.Camera.main.transform.right;
            CamForward.y = 0f;
            CamRight.y = 0f;
            CamForward.Normalize();
            CamRight.Normalize();
        }

        public IEnumerator CountdownToDeath()
        {
            Debug.Log("empieza la corutina");
            yield return new WaitForSeconds(CdToDeath);
            LevelManager.GameOver();     
            yield return null;
        }
    }
}

