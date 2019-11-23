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

        ItemInfo GetAttachedItem(GameObject worldItem)
        {
            string itemName = worldItem.GetComponent<ItemInfo>().itemName;

            ItemInfo attachedItem = inventoryItems.Find( tempItemInfo => tempItemInfo.itemName == itemName);

            return attachedItem;
        }

        public void GetItem(GameObject worldItem)
        {
            ItemInfo attachedItem = GetAttachedItem(worldItem);

            attachedItem.gameObject.SetActive(true);
            GameObject.Destroy(worldItem);

            variableStorage.SetValue(attachedItem.variableName, true);
        }
    }
}