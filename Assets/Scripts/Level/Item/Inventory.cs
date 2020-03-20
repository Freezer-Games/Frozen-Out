using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item
{
    public class Inventory : MonoBehaviour
    {

        public List<ItemInfo> InventoryItems
        {
            private set;
        }

        private IDialogueManager DialogueManager;

        void Awake()
        {
            InventoryItems = new List<ItemInfo>(
                this.gameObject.GetComponentsInChildren<ItemInfo>(true)
            );
        }

        void Start()
        {
            DialogueManager = GameManager.Instance.CurrentLevelManager.GetDialogueManager();
        }

        private ItemInfo GetInventoryItem(GameObject worldItem)
        {
            string itemName = worldItem.GetComponent<ItemInfo>().Name;

            ItemInfo attachedItem = InventoryItems.Find( tempItemInfo => tempItemInfo.Name == itemName);

            return attachedItem;
        }

        public void GetItem(GameObject worldItem)
        {
            ItemInfo inventoryItemInfo = GetInventoryItem(worldItem);

            inventoryItemInfo.gameObject.SetActive(true);
            worldItem.SetActive(false);

            DialogueManager.SetVariable(inventoryItemInfo.VariableName, true);
        }

        public void UseInventoryItem(ItemInfo itemInfo)
        {
            itemInfo.gameObject.SetActive(false);

            DialogueManager.SetVariable(itemInfo.VariableName, false);
            DialogueManager.SetVariable(itemInfo.UsedVariableName, true);
        }

        public bool IsItemInInventory(ItemInfo itemInfo)
        {
            return DialogueManager.GetBoolVariable(itemInfo.VariableName);
        }

        public bool IsItemUsed(ItemInfo itemInfo)
        {
            return DialogueManager.GetBoolVariable(itemInfo.UsedVariableName);
        }
    }
}