using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Dialogue;

namespace Assets.Scripts.Item
{
    public class Inventory : MonoBehaviour
    {
        public List<ItemInfo> inventoryItems
        {
            get;
            private set;
        }

        private VariableStorageYarn variableStorage;

        void Awake()
        {
            inventoryItems = new List<ItemInfo>(
                this.gameObject.GetComponentsInChildren<ItemInfo>(true)
            );
        }

        // Start is called before the first frame update
        void Start()
        {
            variableStorage = FindObjectOfType<VariableStorageYarn>();
        }

        private ItemInfo GetInventoryItem(GameObject worldItem)
        {
            string itemName = worldItem.GetComponent<ItemInfo>().itemName;

            ItemInfo attachedItem = inventoryItems.Find( tempItemInfo => tempItemInfo.itemName == itemName);

            return attachedItem;
        }

        public void GetItem(GameObject worldItem)
        {
            ItemInfo inventoryItemInfo = GetInventoryItem(worldItem);

            inventoryItemInfo.gameObject.SetActive(true);
            worldItem.SetActive(false);

            variableStorage.SetValue(inventoryItemInfo.variableName, true);
        }

        public void UseInventoryItem(ItemInfo itemInfo)
        {
            itemInfo.gameObject.SetActive(false);

            variableStorage.SetValue(itemInfo.variableName, false);
            variableStorage.SetValue(itemInfo.usedVariableName, true);
        }

        public bool IsItemInInventory(ItemInfo itemInfo)
        {
            return variableStorage.GetBoolValue(itemInfo.variableName);
        }

        public bool IsItemUsed(ItemInfo itemInfo)
        {
            return variableStorage.GetBoolValue(itemInfo.usedVariableName);
        }
    }
}