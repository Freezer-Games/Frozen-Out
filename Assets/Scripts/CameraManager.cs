using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public GameObject cameraBase;
    public Camera cinematicCamera;
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
            cinematicPos = GameObject.Find("CinemaCamBase").transform.GetChild(0);
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
        //CameraDialogue();
    }

    void LateUpdate() 
    {
        if (PlayerManager.instance.inCinematic) //Input.GetKey(KeyCode.C)
        {
            ChangeToCinematic();
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
        Camera.main.transform.position = cinematicPos.position;
    }

    public void ChangeToNormal()
    {
        Camera.main.transform.position = normalPos.position;
    }

    public void DisableController()
    {
        cameraBase.GetComponent<CameraFollow>().enabled = false;
        auxCamPos.GetComponent<RotateAround>().enabled = false;
    }

    public void EnableController()
    {
        cameraBase.GetComponent<CameraFollow>().enabled = true;
        auxCamPos.GetComponent<RotateAround>().enabled = true;
    }

    public void CreateTemporalCamera() 
    {
        Instantiate(cinematicCamera, cinematicPos.position, cinematicPos.rotation);
    }

    public void UnableTemporalCamera() 
    {
        cinematicCamera.enabled = false;
        Debug.Log("desactivando");
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
