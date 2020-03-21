using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item
{
    public class Inventory : MonoBehaviour
    {

        public List<Item> InventoryItems
        {
            get;
            private set;
        }

        private IDialogueManager DialogueManager;

        void Awake()
        {
            InventoryItems = new List<Item>(
                this.gameObject.GetComponentsInChildren<Item>(true)
            );
        }

        void Start()
        {
            DialogueManager = GameManager.Instance.CurrentLevelManager.GetDialogueManager();
        }

        private Item GetInventoryItem(GameObject worldItem)
        {
            string itemName = worldItem.GetComponent<Item>().Name;

            Item attachedItem = InventoryItems.Find( tempItemInfo => tempItemInfo.Name == itemName);

            return attachedItem;
        }

        public void GetItem(GameObject worldItem)
        {
            Item inventoryItemInfo = GetInventoryItem(worldItem);

            inventoryItemInfo.gameObject.SetActive(true);
            worldItem.SetActive(false);

            DialogueManager.SetVariable(inventoryItemInfo.VariableName, true);
        }

        public void UseInventoryItem(Item item)
        {
            item.gameObject.SetActive(false);

            DialogueManager.SetVariable(item.VariableName, false);
            DialogueManager.SetVariable(item.UsedVariableName, true);
        }

        public bool IsItemInInventory(Item item)
        {
            return DialogueManager.GetBoolVariable(item.VariableName);
        }

        public bool IsItemUsed(Item item)
        {
            return DialogueManager.GetBoolVariable(item.UsedVariableName);
        }
    }
}