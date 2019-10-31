using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayeController : MonoBehaviour
{
    //public Rigidbody rb;
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    private Vector3 moveDirection;
    public float gravityScale;

    private DialogueRunner dialogueSystemYarn;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
       controller = GetComponent<CharacterController>();

        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        // Remove all player control when we're in dialogue
        if (dialogueSystemYarn.isDialogueRunning == true) {
            return;
        }

        //rb.velocity = new Vector3(Input.GetAxis("Horizontal")*moveSpeed,rb.velocity.y,  Input.GetAxis("Vertical")*moveSpeed);

        /*if(Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }*/

        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y,  Input.GetAxis("Vertical") * moveSpeed);
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + 
                        (transform.right * Input.GetAxis("Horizontal"));

        moveDirection = moveDirection.normalized * moveSpeed; 
        moveDirection.y = yStore;

        if (controller.isGrounded) 
        {
            moveDirection.y = 0; 
            if(Input.GetButtonDown("Jump")) 
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }
}
