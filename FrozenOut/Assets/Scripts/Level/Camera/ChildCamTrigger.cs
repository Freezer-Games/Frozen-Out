using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Level.Camera 
{
    public class ChildCamTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("entra el jugador");
                ParentCamTrigger parent = GetComponentInParent<ParentCamTrigger>();
                if (parent != null)
                    parent.ChangeCamPriority();
            }
        }
    }
}


