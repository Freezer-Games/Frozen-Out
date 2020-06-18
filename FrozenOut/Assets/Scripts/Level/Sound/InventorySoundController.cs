using UnityEngine;
using System.Collections;

namespace Scripts.Level.Sound
{
    public abstract class SoundController : MonoBehaviour
    {
        public AudioSource AudioSource;

        public void PlayClip(AudioClip clip)
        {
            // TODO remove popping noise
            AudioSource.Pause();
            AudioSource.clip = clip;
            AudioSource.Play();
        }

        public void PlayClipOnce(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }

        public void Stop()
        {
            AudioSource.Stop();
        }
    }

    public class InventorySoundController : SoundController
    {
        public AudioClip Pasar;
        public AudioClip Seleccion;
    }
}