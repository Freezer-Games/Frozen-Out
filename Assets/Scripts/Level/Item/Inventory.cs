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
        public ItemPickPromptController ItemPickPromptController;
        public ItemUsePromptController ItemUsePromptController;

        public List<ItemInfo> Items
        {
            get;
            private set;
        }
        private ItemInfo EquippedItem;

        private IDialogueManager DialogueManager => LevelManager.GetDialogueManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        void Awake()
        {
            Items = new List<ItemInfo>();
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
            ItemUsePromptController.Open(usableItem);
        }

        public void CloseUsePrompt()
        {
            ItemUsePromptController.Close();
        }

        public void OpenPickPrompt(ItemInfo item)
        {
            ItemPickPromptController.Open(item);
        }

        public void ClosePickPrompt()
        {
            ItemPickPromptController.Close();
        }

        public KeyCode GetInteractKey()
        {
            return SettingsManager.InteractKey;
        }

        public KeyCode GetInventoryKey()
        {
            return SettingsManager.InventoryKey;
        }

        public bool EquipItem(ItemInfo item)
        {
            if(IsItemInInventory(item) && item.IsEquippable)
            {
                if(IsItemEquipped(item))
                {
                    EquippedItem = null;
                    return false;
                }
                else
                {
                    EquippedItem = item;
                    return true;
                }
            }
            return false;
        }

        public void PickItem(ItemInfo pickedItem)
        {
            pickedItem.OnPickup();
            if(IsItemInInventory(pickedItem))
            {
                UpdateItem(pickedItem);
            }
            else
            {
                AddItem(pickedItem);
            }
        }

        public void UseItem(ItemInfo usedItem)
        {
            if(IsItemInInventory(usedItem))
            {
                if(usedItem.IsEquippable)
                {
                    if(IsItemEquipped(usedItem))
                    {
                        UseEquippedItem();
                    }
                }
                else
                {
                    UseConsumableItem(usedItem);
                }
            }
        }

        private void AddItem(ItemInfo item)
        {
            DialogueManager.SetVariable<bool>(item.VariableName, true);

            Items.Add(item);
        }

        private void UpdateItem(ItemInfo item)
        {
            if(item.Quantity > 0)
            {
                int currentQuantity = (int) DialogueManager.GetNumberVariable(item.QuantityVariableName);
                int newQuantity = currentQuantity + item.Quantity;

                DialogueManager.SetVariable<float>(item.QuantityVariableName, item.Quantity);
                ItemInfo temp = Items.Find( tempItemInfo => tempItemInfo.Name == item.Name);
                temp.Quantity = newQuantity;
            }
        }

        private void UseConsumableItem(ItemInfo consumableItem)
        {
            DialogueManager.SetVariable<bool>(consumableItem.VariableName, false);
            DialogueManager.SetVariable<bool>(consumableItem.UsedVariableName, true);
            DialogueManager.SetVariable<float>(consumableItem.QuantityVariableName, 0);

            Items.Remove(consumableItem);
        }

        private void UseEquippedItem()
        {
            //TODO
        }

        public bool IsItemInInventory(ItemInfo item)
        {
            return DialogueManager.GetBoolVariable(item.VariableName);
        }

        public bool IsItemEquipped(ItemInfo item)
        {
            return EquippedItem != null && item.Name == EquippedItem.Name;
        }

        public bool IsItemUsed(ItemInfo item)
        {
            return DialogueManager.GetBoolVariable(item.UsedVariableName);
        }
        
    }
}