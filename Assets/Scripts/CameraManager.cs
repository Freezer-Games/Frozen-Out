using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public GameObject cameraBase;
    public GameObject auxCamPos;

    private DialogueRunner dialogueSystemYarn;
    public const float NORMALFOV = 60f;
    public const float DIALOGFOV = 30f;
    private float currentFOV;

    public Transform normalPos;
    public Transform cinematicPos;

    void Awake()
    {
        MakeSingleton();
        try{
            cameraBase = GameObject.Find("CameraBase");
            auxCamPos = GameObject.Find("AuxCamPos");
            normalPos = auxCamPos.transform.GetChild(1);
            cinematicPos = auxCamPos.transform.GetChild(0);

        } catch {}
    }

    void Start()
    {
        currentFOV = NORMALFOV;
        try 
        {
            dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        } catch {}
    }

    void Update() 
    {
        CameraDialogue();
    }

    void LateUpdate() 
    {
        cinematicPos = GameObject.Find("AuxCamPos").transform.GetChild(0);
        if (PlayerManager.instance.inCinematic) //Input.GetKey(KeyCode.C)
        {
            ChangeToCinematic();
        }
        else 
        {
            
        }
    }

    void CameraDialogue() {
        if (dialogueSystemYarn.isDialogueRunning && dialogueSystemYarn.currentNodeName != "Guardia") 
        {
            if (currentFOV > DIALOGFOV) currentFOV -= 1;
        }
        else 
        {
            if (currentFOV < NORMALFOV) currentFOV += 1;
         }

        Camera.main.fieldOfView = currentFOV;
    }

    void ChangeToCinematic() 
    {
        Debug.Log("haciendo el cambio");
        //cam.transform.position = Vector3.Lerp(cam.transform.position, cinemaPos.position, transitionSpeed*Time.deltaTime);
        cameraBase.transform.position = cinematicPos.position;
    }

    public void DisableController()
    {
        cameraBase.GetComponent<CameraFollow>().enabled = false;
    }

    public void EnableController()
    {
        cameraBase.GetComponent<CameraFollow>().enabled = true;
    }

    protected void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
