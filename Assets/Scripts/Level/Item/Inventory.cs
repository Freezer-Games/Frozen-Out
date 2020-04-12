using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Dialogue;

namespace Scripts.Level.Item
{
    public class Inventory : MonoBehaviour
    {
        public LevelManager LevelManager;
        
        public UIController InventoryMenuController;
        public InventoryUIController ItemPickPromptController;
        public InventoryUIController ItemUsePromptController;

        public List<ItemInfo> Items
        {
            get;
            private set;
        }
        public ItemInfo EquippedItem
        {
            get;
            private set;
        }

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

        /// Equipa el item si no estaba equipado, no hace nada si ya lo estaba
        public void EquipItem(ItemInfo item)
        {
            if(IsItemInInventory(item) && item.IsEquippable)
            {
                EquippedItem = item;

                OnItemEquipped(item);
            }
        }

        /// Desequipa el item equipado
        public void UnequipItem()
        {
            EquippedItem = null;

            OnItemUnequipped();
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
            if(item.Quantity > 0)
            {
                DialogueManager.SetVariable<float>(item.QuantityVariableName, item.Quantity);
            }

            Items.Add(item);

            OnItemAdded(item);
        }

        private void UpdateItem(ItemInfo item)
        {
            if(item.Quantity > 0)
            {
                int currentQuantity = (int) DialogueManager.GetNumberVariable(item.QuantityVariableName);
                int newQuantity = currentQuantity + item.Quantity;

                DialogueManager.SetVariable<float>(item.QuantityVariableName, item.Quantity);
                ItemInfo inventoryItem = Items.Find( tempItemInfo => tempItemInfo.Name == item.Name);
                inventoryItem.Quantity = newQuantity;

                OnItemUpdated(inventoryItem);
            }
        }

        private void UseConsumableItem(ItemInfo consumableItem)
        {
            DialogueManager.SetVariable<bool>(consumableItem.VariableName, false);
            DialogueManager.SetVariable<bool>(consumableItem.UsedVariableName, true);
            DialogueManager.SetVariable<float>(consumableItem.QuantityVariableName, 0);

            Items.Remove(consumableItem);

            OnItemRemoved(consumableItem);
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

        #region Events
        public event EventHandler<ItemEventArgs> ItemAdded;
        public event EventHandler<ItemEventArgs> ItemRemoved;
        public event EventHandler<ItemEventArgs> ItemUpdated;
        public event EventHandler<ItemEventArgs> ItemEquipped;
        public event EventHandler ItemUnequipped;

        private void OnItemAdded(ItemInfo itemAdded)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemAdded);
            ItemAdded?.Invoke(this, itemEventArgs);
        }

        private void OnItemRemoved(ItemInfo itemRemoved)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemRemoved);
            ItemRemoved?.Invoke(this, itemEventArgs);
        }

        private void OnItemUpdated(ItemInfo itemUpdated)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemUpdated);
            ItemUpdated?.Invoke(this, itemEventArgs);
        }

        private void OnItemEquipped(ItemInfo item)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(item);
            ItemEquipped?.Invoke(this, itemEventArgs);
        }

        private void OnItemUnequipped()
        {
            ItemUnequipped?.Invoke(this, EventArgs.Empty);
        }
        #endregion
        
    }

    [Serializable]
    public class ItemEventArgs : EventArgs
    {

        public ItemInfo Item
        {
            get;
        }

        public ItemEventArgs(ItemInfo itemInfo)
        {
            this.Item = itemInfo;
        }

    }
}