using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Audio
{
    public class MusicController : MonoBehaviour
    {
        public float VolumeReduceFactor = 0.4f;

        private AudioSource source;
        private DialogueRunner dialogueSystem;

        void Start()
        {
            source = GetComponent<AudioSource>();
            dialogueSystem = FindObjectOfType<DialogueRunner>();

            dialogueSystem.Started += (s, e) => ReduceVolume();
            dialogueSystem.Ended += (s, e) => IncreaseVolume();
        }

        private void ReduceVolume()
        {
            source.volume *= VolumeReduceFactor;
        }

        private void IncreaseVolume()
        {
            source.volume /= VolumeReduceFactor;
        }
    }
}
