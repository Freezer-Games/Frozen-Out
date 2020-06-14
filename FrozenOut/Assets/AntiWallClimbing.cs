using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player
{
    public class AntiWallClimbing : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        public NormalController controller;

        void OnCollisionEnter(Collision other)
        {
            Debug.Log("solo choca collider auxiliar");
            if (!controller.Grounded)
            {
                Rigidbody.AddForce(new Vector3(0f, 4f, 0f));
            }
        }
    }
}

