using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TuttiMovement : MonoBehaviour
{
    private Vector3 objective = Vector3.zero;
    
    private const float SPEED = 5f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (objective != Vector3.zero) {
            float step = SPEED * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, objective, step);
        }
    }

    public void SetPoints(Vector3 obj) {
        objective = obj;
    }
}
