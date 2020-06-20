using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Level.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundController : MonoBehaviour
    {
        public AudioSource AudioSource;

        public void PlayClip(AudioClip clip)
        {
            AudioSource.PlayOneShot(clip);
        }

        public void PlayRandomClip(ICollection<AudioClip> clips)
        {
            AudioClip randomClip = RandomElement(clips);

            PlayClip(randomClip);
        }

        public void Stop()
        {
            AudioSource.Stop();
        }

        protected T RandomElement<T>(ICollection<T> collection)
        {
            T randomElement;
            if (collection.Count() > 1)
            {
                int randomIndex = Random.Range(0, collection.Count());
                randomElement = collection.ElementAt(randomIndex);
            }
            else
            {
                randomElement = collection.FirstOrDefault();
            }

            return randomElement;
        }
    }
}