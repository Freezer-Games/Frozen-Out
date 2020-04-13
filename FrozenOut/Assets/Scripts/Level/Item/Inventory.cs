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
        public ItemPickPromptController ItemPickPromptController;
        public ItemUsePromptController ItemUsePromptController;

        public List<ItemInfo> LevelItems;

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

        public void OpenUsePrompt(ItemUser user)
        {
            ItemUsePromptController.Open(user);
        }

        public void CloseUsePrompt()
        {
            ItemUsePromptController.Close();
        }

        public void OpenPickPrompt(ItemPicker picker)
        {
            ItemPickPromptController.Open(picker);
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

        public void PickItem(ItemPicker picker)
        {
            picker.OnPickup();

            if(IsItemInInventory(picker.ItemName))
            {
                UpdateItem(picker);
            }
            else
            {
                AddItem(picker);
            }
        }

        public void UseItem(ItemUser user)
        {
            if(IsItemInInventory(user.ItemName))
            {
                user.OnUse();
                //Pasarlo a ItemInfo del inventario
                ItemInfo inventoryItem = Items.Find( temp => temp.Name == user.ItemName );

                if(inventoryItem.IsEquippable)
                {
                    if(IsItemEquipped(inventoryItem))
                    {
                        UseEquippedItem();
                    }
                }
                else
                {
                    UseConsumableItem(inventoryItem);
                }
            }
        }

        private void AddItem(ItemPicker picker)
        {
            //Pasarlo a ItemInfo del nivel
            ItemInfo item = LevelItems.Find( temp => temp.Name == picker.ItemName );
            item.Quantity = picker.ItemQuantity;

            Items.Add(item);
            DialogueManager.SetVariable<bool>(item.VariableName, true);
            DialogueManager.SetVariable<float>(item.QuantityVariableName, item.Quantity);

            OnItemAdded(item);
        }

        private void UpdateItem(ItemPicker picker)
        {
            if(picker.ItemQuantity > 0)
            {
                //Pasarlo a ItemInfo del inventario
                ItemInfo inventoryItem = Items.Find( temp => temp.Name == picker.ItemName );

                int currentQuantity = inventoryItem.Quantity;
                int newQuantity = currentQuantity + picker.ItemQuantity;

                inventoryItem.Quantity = newQuantity;
                DialogueManager.SetVariable<float>(inventoryItem.QuantityVariableName, newQuantity);

                OnItemUpdated(inventoryItem);
            }
        }

        private void UseConsumableItem(ItemInfo consumableItem)
        {
            Items.Remove(consumableItem);

            DialogueManager.SetVariable<bool>(consumableItem.VariableName, false);
            DialogueManager.SetVariable<bool>(consumableItem.UsedVariableName, true);
            DialogueManager.SetVariable<float>(consumableItem.QuantityVariableName, 0);

            OnItemRemoved(consumableItem);
        }

        private void UseEquippedItem()
        {
            //TODO
        }

        public bool IsItemInInventory(ItemInfo item)
        {
            return Items.Contains(item);
        }
        public bool IsItemInInventory(string itemName)
        {
            return Items.Exists( temp => temp.Name == itemName );
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