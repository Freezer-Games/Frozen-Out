using System.Collections;
using UnityEngine;

using Scripts.Level.Sound;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item
{
    public class OreItem : ItemUser
    {
        public OreSoundController SoundController;
        public DialogueActer UnableTalker;

        [SerializeField] float animDelay;

        public ItemPickerInfo Ice;
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();
        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        public override void OnPlayerCol()
        {

        }

        public override void OnPlayerExitCol()
        {
               
        }

        public override void OnUse()
        {
            GetComponent<Collider>().enabled = false;

            StartCoroutine(PlayParticles());
            Inventory.PickItem(Ice);
            Ice.Quantity = 0;
        }

        public override void OnUnableUse()
        {
            DialogueManager.StartDialogue(UnableTalker);
        }

        IEnumerator PlayParticles()
        {
            yield return new WaitForSeconds(animDelay);
            SoundController.PlayRandomClip(SoundController.Ores);
            DestroyItem();
            yield return null;
        }
    }
}
