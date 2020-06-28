using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Scripts.Level.Item
{
    public class LeverUserListener : MonoBehaviour
    {
        public ItemBase LeverItem;
        public Animator AscensorAnimator;
        public PlayableDirector Timeline;

        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;
        private Inventory Inventory => LevelManager.GetInventory();

        void Start()
        {
            Inventory.ItemUsed += (sender, args) => OnItemUsed(args.Item);
        }

        private void OnItemUsed(ItemInfo item)
        {
            if (item.Equals(LeverItem))
            {
                StartCoroutine(ActivateAscensor());
            }
        }

        private IEnumerator ActivateAscensor()
        {
            AscensorAnimator.SetTrigger("active");
            yield return new WaitForSeconds(0.75f);
            Timeline.Play();
            //yield return null;
        }
    }
}