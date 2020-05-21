using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Player
{
    public class Stalactite : MonoBehaviour
    {
        Rigidbody rb;
        [SerializeField] private MoveMode reactTo;
        public LayerMask whatIsGround;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.Sleep();
        }

        void OnCollisionEnter(Collision other)
        {
            if (whatIsGround == (whatIsGround | (1 << other.gameObject.layer)))
            {
                rb.Sleep();
                Debug.Log("Colision suelo");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player encontrado");
                GameObject player = other.gameObject;
                /*if (player.GetComponentInParent<PlayerController>().GetMoveStatus() == reactTo)
                {
                    rb.WakeUp();
                }*/
            }
        }
    }
}