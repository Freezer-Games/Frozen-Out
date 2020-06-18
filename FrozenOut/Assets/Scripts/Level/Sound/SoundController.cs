using UnityEngine;

namespace Scripts.Level.Sound
{
    public abstract class SoundController : MonoBehaviour
    {
        public static float VolumeReduceFactor = 0.4f;

        public AudioSource AudioSource;

        public void PlayClip(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }

        public void Stop()
        {
            AudioSource.Stop();
        }
    }
}