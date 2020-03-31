using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private MoveMode reactTo;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8 && rb.isKinematic == true)
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
            if (player.GetComponentInParent<PlayerController>().getMoveStatus() == reactTo)
            {
                rb.WakeUp();
            }
        }
    }
}
