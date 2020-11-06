using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Player 
{
    public class PlayerBase: MonoBehaviour
    {
        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        public PlayerManager PlayerManager;
        public NormalController NormalController;
        public MeltedController MeltedController;

        [Header("Componentes")]
        public Rigidbody Rigidbody;
        public Animator Animator;
        public CapsuleCollider Collider;

        [Header("States")]
        public bool IsInteracting;
        public bool CanMove;
        public bool Grounded;

        [Header("Interact")]
        public Transform InteractPos;
        public Transform InteractLook;

        [Header("Layer Checker")]
        [SerializeField] LayerMask DeathZone;
        [SerializeField] LayerMask Ground;
        [SerializeField] LayerMask Ignored;
        [SerializeField] float groundDistance;

        [Header("Health")]
        [SerializeField] float MaxHealth;
        [SerializeField] float ChangeHealthTime;
        float Health = 100;
        private bool Dead = false;
        public bool InDeathZone;
        private Coroutine deathCoroutine;

        private Vector2 MoveInput;
        private Vector3 Movement;
        private Vector3 CamForward;
        private Vector3 CamRight;

        [SerializeField] private GameObject PlayerHealthUI;
        private SpriteRenderer DeteccionSprite;
        private Renderer UIRenderer;
        private MaterialPropertyBlock _propBlock;

        [SerializeField] private GameObject MissionsUI;

        void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            Collider = GetComponent<CapsuleCollider>();

            DeteccionSprite = PlayerHealthUI.GetComponent<SpriteRenderer>();
            DeteccionSprite.enabled = true;
            UIRenderer = PlayerHealthUI.GetComponent<Renderer>();
            _propBlock = new MaterialPropertyBlock();
            _propBlock.SetFloat("_Change", 1);
            UIRenderer.SetPropertyBlock(_propBlock);
            Health = MaxHealth;
            PlayerHealthUI.SetActive(false);
        }

        void Update()
        {
            if (NormalController.isActiveAndEnabled)
            {
                Collider.center = new Vector3(0f, 1f, 0f);
                Collider.radius = 0.5f;
                Collider.height = 2f;
            }
            else if (MeltedController.isActiveAndEnabled)
            {
                Collider.center = new Vector3(0f, 0.25f, 0f);
                Collider.radius = 0.25f;
                Collider.height = 0.3f;
            }

            if (PlayerManager.IsEnabled())
            {
                if (IsInteracting)
                {
                    CanMove = false;
                    Rigidbody.isKinematic = true;
                    Rigidbody.useGravity = false;

                    MoveToTarget(InteractPos, InteractLook, 0.01f, 0.5f);
                }
                else
                {
                    CanMove = true;
                    Rigidbody.isKinematic = false;
                    Rigidbody.useGravity = true;
                }

                CheckWithRay();

                if (!CanMove)
                {
                    Animator.SetTrigger("isSneakingOut");
                    Animator.SetBool("isMoving", false);
                }

                GetInputs();
            }
            else
            {
                CanMove = false;
                Animator.SetTrigger("isSneakingOut");
                Animator.SetBool("isMoving", false);
            }

            if (Health <= 0 && !Dead) {
                LevelManager.GameOver();
                Dead = true;
            }

            PlayerHealthUI.transform.LookAt(UnityEngine.Camera.main.transform);
        }

        void FixedUpdate()
        {
            CheckWithRay();
        }

        void LateUpdate()
        {
            CameraVectors();    
        }

        public Vector3 GetMovement()
        {
            Movement = MoveInput.x * CamRight + MoveInput.y * CamForward;
            Movement.Normalize();
            return Movement;
        }

        private void GetInputs() 
        {
            MoveInput = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));
            MoveInput.Normalize();
        }

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
                goPos.y = transform.position.y;

                if (Vector3.Distance(goPos, transform.position) > distanceToStop) 
                {     
                    Animator.SetBool("isMoving", true);

                    transform.position = 
                        Vector3.MoveTowards(transform.position, goPos, speed);
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

        private void CameraVectors() 
        {
            CamForward = UnityEngine.Camera.main.transform.forward;
            CamRight = UnityEngine.Camera.main.transform.right;
            CamForward.y = 0f;
            CamRight.y = 0f;
            CamForward.Normalize();
            CamRight.Normalize();
        }

        private void CheckWithRay()
        {
            Debug.DrawRay(transform.position, -transform.up * groundDistance, Color.red, 1f);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundDistance, ~Ignored))
            {
                if (Ground == (Ground | (1 << hit.transform.gameObject.layer)))
                {
                    Grounded = true;
                    Animator.SetBool("isGrounded", true);

                    if (DeathZone == (DeathZone | (1 << hit.transform.gameObject.layer)))
                    {
                        if (!InDeathZone)
                        {
                            InDeathZone = true;
                            PlayerHealthUI.SetActive(true);
                            deathCoroutine = StartCoroutine(OnDeathZone());
                            PlayerManager.OnDeathZone();
                        }
                    }else{
                        if (deathCoroutine != null)
                        {
                            StopCoroutine(deathCoroutine);
                        }
                        if (Health < MaxHealth)
                        {
                            deathCoroutine = StartCoroutine(NotOnDeathZone());
                        }
                        else
                        {
                            PlayerHealthUI.SetActive(false);
                        }
                        InDeathZone = false;
                        PlayerManager.OnNormalZone();
                    }

                }
            }
        }

        public void LowerHealth()
        {
            if (!Dead)
            {
                Health--;
                //Debug.Log("Health: " + Health);
                UIRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat("_Change", 1 - (MaxHealth - Health) / MaxHealth);
                UIRenderer.SetPropertyBlock(_propBlock);
            }
        }

        public void BoostHealth()
        {
            if (!Dead)
            {
                Health++;
                //Debug.Log("Health: " + Health);
                UIRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat("_Change", 1 - (MaxHealth - Health) / MaxHealth);
                UIRenderer.SetPropertyBlock(_propBlock);
            }
        }

        private IEnumerator OnDeathZone()
        {
            LowerHealth();
            yield return new WaitForSeconds(ChangeHealthTime);
            if (InDeathZone)
            {
                deathCoroutine = StartCoroutine(OnDeathZone());
            }
        }

        private IEnumerator NotOnDeathZone()
        {
            BoostHealth();
            yield return new WaitForSeconds(ChangeHealthTime);
            if (!InDeathZone && Health != MaxHealth)
            {
                deathCoroutine = StartCoroutine(NotOnDeathZone());
            }
            else if (!InDeathZone && Health >= MaxHealth) {
                PlayerHealthUI.SetActive(false);
            }
        }
    }
}

