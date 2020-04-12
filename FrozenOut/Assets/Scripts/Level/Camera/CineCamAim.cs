using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Camera
{
    public class CineCamAim : MonoBehaviour
    {
        
        public Transform Target;
        public Transform Position;

        void Update()
        {
            transform.position = Position.position;
            try {
                transform.LookAt(Target);
            } catch {}
        }

        public void SetTarget(Transform t) 
        {
            Target = t;
        }

    }
}