using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Dialogue;

public class TuttiEscapeTrigger : MonoBehaviour
{
    private bool entered = false;
    [SerializeField]
    private string valueField;
    
    void OnTriggerEnter(Collider other)
    {
        if(!entered && other.CompareTag("Player"))
        {
            entered = true;
            FindObjectOfType<VariableStorageYarn>().SetValue<bool>(valueField, true);
        }
    }
}
