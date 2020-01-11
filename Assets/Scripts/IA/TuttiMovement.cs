using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TuttiMovement : MonoBehaviour
{
    public Vector3 target = Vector3.zero;
    
    private const float SPEED = 5f;

    // Update is called once per frame
    void Update()
    {
        if (target != Vector3.zero) {
            float step = SPEED * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            GameManager.instance.blackBars.GetComponent<CinematicBars>().Hide(.3f);
            CameraManager.instance.ChangeToNormal();
            PlayerManager.instance.EnableController();
            CameraManager.instance.EnableController();
            PlayerManager.instance.inCinematic = false;
            Destroy(gameObject);
        }
    }

    public void SetPoint(Vector3 obj) {
        target = obj;
    }
}
