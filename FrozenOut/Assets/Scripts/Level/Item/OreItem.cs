using System.Collections;
using UnityEngine;

using Scripts.Level.Sound;

namespace Scripts.Level.Item
{
    public class OreItem : ItemUser
    {
        public ParticleSystem Particles;
        public OreSoundController SoundController;

        [SerializeField] float animDelay;
        [SerializeField] float particlesDelay = 1f;

        //public ItemPickerInfo Ice;
        //private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        public override void OnPlayerAway()
        {
            
        }

        public override void OnPlayerClose()
        {
        }

        public override void OnPlayerCol()
        {

        }

        public override void OnPlayerExitCol()
        {
               
        }

        public override void OnUse()
        {
            StartCoroutine(PlayParticles());
            //Inventory.PickItem(Ice);
        }

        IEnumerator PlayParticles()
        {
            yield return new WaitForSeconds(animDelay);
            Particles.Play();
            SoundController.PlayRandomClip(SoundController.Ores);
            yield return new WaitForSeconds(particlesDelay);
            DestroyItem();
            yield return null;
        }
    }
}
