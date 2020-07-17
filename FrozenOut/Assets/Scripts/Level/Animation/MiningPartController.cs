using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level.Animation
{
    public class MiningPartController : MonoBehaviour
    {
        public GameObject MiningParticles;

        public void ManageParticles()
        {
            StartCoroutine(PlayParticles());
        }

        private IEnumerator PlayParticles()
        {
            MiningParticles.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            MiningParticles.SetActive(false);
        }
    }
}