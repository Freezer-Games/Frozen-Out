using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public GameObject cameraBase;
    public GameObject auxCamPos;

    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject cinematicCamera;
    
    private DialogueRunner dialogueSystemYarn;


    [Header("Camera positions")]
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
        try 
        {
            dialogueSystemYarn = FindObjectOfType<DialogueRunner>();
        } catch {}
    }

    void LateUpdate() 
    {
        if (PlayerManager.instance.inCinematic)
        {
            ChangeToCinematic();
        }
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

    public void ToNormalCamera()
    {
        mainCamera.SetActive(true);
        cinematicCamera.SetActive(false);
    }

    public void ToCinemaCamera()
    {
        mainCamera.SetActive(false);
        cinematicCamera.SetActive(true);
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
