using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDetectionUIAnimation: MonoBehaviour
{
    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartAnimation()
    {

        anim.SetBool("Detectado", true);

    }

    public void StopAnimation()
    {

        anim.SetBool("Detectado", false);

    }

}
