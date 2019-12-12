using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraFollow : MonoBehaviour
{
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 FollowPos;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public float camDistanceXTopPlayer;
    public float camDistanceYTopPlayer;
    public float camDistanceZTopPlayer;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    private float transitionSpeed = 1.0f;
    private Transform cinemaPos;
    private Camera cam;
    //private Vector3 lastPos;
    private DialogueRunner dialogueSystemYarn;
    public const float NORMALFOV = 60f;
    public const float DIALOGFOV = 30f;
    private float currentFOV;

    void Start()
    {
        cam = Camera.main;
        currentFOV = NORMALFOV;
        cinemaPos = GameObject.Find("AuxCamPos").transform.GetChild(0);
        dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    // Update is called once per frame
    void Update()
    {
        
        //para pillar controles de mando hay que crear un axis de esos, cosa easy si sabes la distribucion
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX;
        finalInputZ = mouseY;

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        CameraDialogue();

        /*if (GameManager.instance.inCinematic || Input.GetKey(KeyCode.C)) {
            ChangeToCinematic();
        }*/
    }

    void LateUpdate() 
    {
        cinemaPos = GameObject.Find("AuxCamPos").transform.GetChild(0);
        if (GameManager.instance.inCinematic) //Input.GetKey(KeyCode.C)
        {
            ChangeToCinematic();
        }
        else 
        {
            CameraUpdater();
        }
        
    }

    void CameraUpdater() 
    {
        Transform target = CameraFollowObj.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

    void CameraDialogue() {
        if (dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia") 
        {
            if (currentFOV > DIALOGFOV) 
            {
                currentFOV -= 1;
            }
        }
        else 
        {
            if (currentFOV < NORMALFOV)
            {
                currentFOV += 1;
            }

         }
        Camera.main.fieldOfView = currentFOV;
    }

    void ChangeToCinematic() 
    {
        Debug.Log("haciendo el cambio");
        //cam.transform.position = Vector3.Lerp(cam.transform.position, cinemaPos.position, transitionSpeed*Time.deltaTime);
        cam.transform.position = cinemaPos.position;
    }
}
