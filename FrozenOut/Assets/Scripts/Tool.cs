using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    public PlayerController controller;

    void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && controller.obstacleAtFront != null && controller.hasPickaxe)
        {
            Destroy(controller.obstacleAtFront);
        }
    }
}
