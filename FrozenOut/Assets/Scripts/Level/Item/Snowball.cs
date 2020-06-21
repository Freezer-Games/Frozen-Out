using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    private Rigidbody Rigidbody;
    public Vector3 RbVelocity;
    public float growtRate = 5.0f;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RbVelocity = Rigidbody.velocity;

        if (transform.localScale.x < 123)
        {
            if (RbVelocity != Vector3.zero)
            {
                var growth = growtRate * Time.deltaTime;
                transform.localScale += new Vector3(growth, growth, growth);
            }
        }
    }
}
