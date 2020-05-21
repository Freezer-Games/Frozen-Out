using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player
{
    public class Tool : MonoBehaviour
    {
        public PlayerController controller;

        void Awake()
        {
            controller = GetComponentInParent<PlayerController>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && controller.InteractItem != null && controller.HasPickaxe)
            {
                Destroy(controller.InteractItem);
            }
        }
    }
}