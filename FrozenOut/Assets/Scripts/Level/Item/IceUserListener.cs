using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class IceUserListener : MonoBehaviour
    {
        public ItemBase IceItem;

        private ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;
        private Inventory Inventory => LevelManager.GetInventory();

        void Start()
        {
            Inventory.ItemUsed += (sender, args) => OnItemUsed(args.Item);
        }

        private void OnItemUsed(ItemInfo item)
        {
            if(item.Equals(IceItem))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}