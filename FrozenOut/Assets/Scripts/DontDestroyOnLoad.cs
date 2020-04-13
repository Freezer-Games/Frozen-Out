using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}