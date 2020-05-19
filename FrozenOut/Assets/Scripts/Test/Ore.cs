using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Interactive
{
    [SerializeField] int Durability;
    public ParticleSystem Particles;
    public GameObject Nugget;

    public override void Execute()
    {
        Durability--;

        if (Durability <= 0)
        {
            StartCoroutine(PlayParticles());
        }
    }

    public override void Advise()
    {
        //abre ventana de si no tienes lo necesario o algo
        throw new System.NotImplementedException();
    }

    IEnumerator PlayParticles()
    {
        yield return new WaitForSeconds(0.5f);
        Particles.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield return null;
    }
}
