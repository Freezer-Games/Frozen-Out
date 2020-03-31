using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item
{
    public class Inventory : MonoBehaviour
    {
        public LevelManager LevelManager;
        
        public InventoryMenuController InventoryMenuController;
        public InventoryUseMenuController InventoryUseMenuController;

        public List<ItemInfo> InventoryItems
        {
            get;
            private set;
        }

        private IDialogueManager DialogueManager => LevelManager.GetDialogueManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        void Awake()
        {
            InventoryItems = new List<ItemInfo>(
                this.gameObject.GetComponentsInChildren<ItemInfo>(true)
            );
        }

        void Start()
        {
            
        }

        public void OpenMenu()
        {
            InventoryMenuController.Open();
        }

        public void CloseMenu()
        {
            InventoryMenuController.Close();
        }

        public void OpenUsePrompt(ItemInfo usableItem)
        {
            InventoryUseMenuController.Open(usableItem);
        }

        public void CloseUsePrompt()
        {
            InventoryUseMenuController.Close();
        }

        public KeyCode GetInteractKey()
        {
            return SettingsManager.InteractKey;
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

            DialogueManager.SetVariable<bool>(inventoryItemInfo.VariableName, true);
        }

        public void UseInventoryItem(ItemInfo item)
        {
            item.gameObject.SetActive(false);

            DialogueManager.SetVariable<bool>(item.VariableName, false);
            DialogueManager.SetVariable<bool>(item.UsedVariableName, true);
        }

        public bool IsItemInInventory(ItemInfo item)
        {
            return DialogueManager.GetBoolVariable(item.VariableName);
        }

        public bool IsItemUsed(ItemInfo item)
        {
            return DialogueManager.GetBoolVariable(item.UsedVariableName);
        }
        
    }
}