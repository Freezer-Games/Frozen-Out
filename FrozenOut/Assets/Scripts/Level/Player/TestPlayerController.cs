using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum MoveMode { Stopped, Stealth, Normal, Running }

public class TestPlayerController : MonoBehaviour
{
    const float STEALTH_SPEED = 3.75f;
    const float NORMAL_SPEED = 4f;
    const float RUN_SPEED = 6.5f;

    [Header("Features")]
    Rigidbody playerRb;
    Animator playerAnim;
    [SerializeField] bool formChanged;
    [SerializeField] bool isGrounded;
    [SerializeField] private MoveMode moveState;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce = 2.3f;
    [SerializeField] float canJumpDist;
    public Transform rayOrigin;
    public Transform body;

    public GameObject pico;
    public Transform toolPoint;
    public GameObject obstacleAtFront;
    public bool hasPickaxe;

    Vector2 input;
    Vector3 movement, camForward, camRight;
    public LayerMask whatIsGround;

    [Header("Events")]
    public UnityEvent OnLandEvent;
    
    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();

        if (OnLandEvent == null) 
            OnLandEvent = new UnityEvent();
    }

    void Start()
    {
        moveState = MoveMode.Stopped;
        moveSpeed = NORMAL_SPEED;
        canJumpDist = 0.51f;
        formChanged = false;
        hasPickaxe = false;
        isGrounded = true;
    }

    void Update()
    {
        input = new Vector2(
        Input.GetAxis("Horizontal"),
        Input.GetAxis("Vertical"));
        input.Normalize();

        playerAnim.SetBool("isMoving", movement != Vector3.zero);
        playerAnim.SetFloat("VelX", input.x);
        playerAnim.SetFloat("VelY", input.y);

        if (Input.GetKeyDown(KeyCode.K)) playerAnim.SetTrigger("isChanging");

        //Check there is an input
        if (movement != Vector3.zero)
        {
            FaceMovement();

            if (Input.GetButton("Run"))
            {
                MoveStateManage(MoveMode.Running);
            }
            else if (Input.GetButton("Crouch"))
            {
                MoveStateManage(MoveMode.Stealth);
            }
            else
            {
                MoveStateManage(MoveMode.Normal);
            }
        }
        else
        {
            MoveStateManage(MoveMode.Stopped);
        }   
    }

    void LateUpdate()
    {
        CameraVectors();
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && CheckGround())
        {
            Jump();
        }

        
        if (!isGrounded)
        {
            //OnLandEvent.Invoke();
            if (CheckGround() || playerRb.velocity.y < 0) 
            {
                Debug.Log("buscando suelo para aterrizar");
                isGrounded = true;
                playerAnim.SetBool("isLanding", true);
            }
        }
            
        

        Move();  
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pickaxe"))
        {
            GameObject tool = Instantiate(pico, toolPoint.position, Quaternion.Euler(0f, 0f, 180f));
            tool.transform.parent = toolPoint.transform;
            hasPickaxe = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            obstacleAtFront = other.gameObject;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            obstacleAtFront = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (whatIsGround == (whatIsGround | (1 << other.gameObject.layer)))
        {
            isGrounded = true;
        }*/
    }

    void OnTriggerExit(Collider other)
    {
        /*if (whatIsGround == (whatIsGround | (1 << other.gameObject.layer)))
        {
            isGrounded = false;
        }*/
    }

    void Move()
    {
        movement = input.x * camRight + input.y * camForward;
        movement.Normalize();
        movement *= moveSpeed;

        playerRb.velocity = new Vector3(movement.x, playerRb.velocity.y, movement.z);
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        playerAnim.SetTrigger("isJumping");
    }

    bool CheckGround() {
        RaycastHit hit;
        Ray checker = new Ray(rayOrigin.position, Vector3.down);
        Debug.DrawRay(rayOrigin.position, Vector3.down, Color.red, 0.1f);

        if (Physics.Raycast(checker, out hit, canJumpDist)) 
        {
            if (hit.collider.CompareTag("Ground")) 
            {
                Debug.Log("dist origin-hit: " + Vector3.Distance(hit.point, rayOrigin.position));
                return true;
            }
            else 
            {
                Debug.Log("aqui no se puede saltar");
                return false;
            }
        } 
        else 
        {
            Debug.Log("en el aire");
            return false;
        }
    }

    bool ChechLanding() 
    {
        return false;
    }

    void MoveStateManage(MoveMode mode)
    {
        if (mode != moveState)
        {
            if (moveState == MoveMode.Stopped && mode == MoveMode.Normal)
            {
                moveState = mode;
                moveSpeed = NORMAL_SPEED;
            }
            else if (moveState == MoveMode.Normal && mode == MoveMode.Stopped)
            {
                moveState = mode;
            }
            else if (moveState == MoveMode.Normal && mode == MoveMode.Running)
            {
                moveState = mode;
                moveSpeed = RUN_SPEED;
            }
            else if (moveState == MoveMode.Running && mode == MoveMode.Normal)
            {
                moveState = mode;
                moveSpeed = NORMAL_SPEED;
            }
            else if (moveState == MoveMode.Normal && mode == MoveMode.Stealth)
            {
                moveState = mode;
                moveSpeed = STEALTH_SPEED;
            }
            else if(moveState == MoveMode.Stealth && mode == MoveMode.Normal)
            {
                moveState = mode;
                moveSpeed = NORMAL_SPEED;
            }
        }
    }

    void FaceMovement()
    {
        body.rotation = 
            Quaternion.Slerp(body.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
    }

    void CameraVectors()
    {
        camForward = Camera.main.transform.forward;
        camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
    }

    public MoveMode getMoveStatus()
    {
        return moveState;
    }
}
