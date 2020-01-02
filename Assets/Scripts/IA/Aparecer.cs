using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Dialogue;

public class Aparecer : MonoBehaviour
{
    private bool objetivo = false;

    [SerializeField]
    private string valueDialog;

    private VariableStorageYarn variableStorageYarn;

    void Awake()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Start()
    {
        variableStorageYarn = FindObjectOfType<VariableStorageYarn>();
    }

    void Update()
    {
        if (!objetivo && variableStorageYarn.GetBoolValue(valueDialog) == true) {
            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            objetivo = true;
        }
    }
}
