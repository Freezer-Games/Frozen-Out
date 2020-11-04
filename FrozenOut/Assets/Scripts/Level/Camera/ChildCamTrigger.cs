using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Level.Camera 
{
    public class ChildCamTrigger : TriggerBase
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(PlayerTag))
            {
                ParentCamTrigger[] parents = GetComponentsInParent<ParentCamTrigger>();
                if (parents.Length > 0)
                {
                    foreach (ParentCamTrigger p in parents)
                    {
                        p.ChangeCamPriority();
                    }
                }
            }
        }
    }
}


