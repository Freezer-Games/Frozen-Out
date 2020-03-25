using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private MoveMode reactTo;
    [SerializeField] private LayerMask layer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
        reactTo = MoveMode.Normal;
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            rb.Sleep();
        }
    }
}
