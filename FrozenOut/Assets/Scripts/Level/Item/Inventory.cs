using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Dialogue;
using Scripts.Level.Player;

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

        private DialogueManager DialogueManager => LevelManager.GetDialogueManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();
        private PlayerManager PlayerManager => LevelManager.GetPlayerManager();

        void Awake()
        {
            Items = new List<ItemInfo>();
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
            PlayerManager.SetInteractiveItem(user.transform, user.GetInteractionPoint());
        }

        public void CloseUsePrompt()
        {
            ItemUsePromptController.Close();
            PlayerManager.SetInteractiveItem(null, null);
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

            if(IsItemInInventory(picker.ToItemInfo()))
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
            if (string.IsNullOrEmpty(user.ItemVariableName))
            {
                user.OnUse();
            }
            else if(IsItemInInventory(user.ToItemInfo()))
            {
                user.OnUse();
                //Pasarlo a ItemInfo del inventario
                ItemInfo inventoryItem = Items.Find( temp => temp.Equals(user.ToItemInfo()) );

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
            ItemInfo item = LevelItems.Find( temp => temp.Equals(picker.ToItemInfo()) );
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
                ItemInfo inventoryItem = Items.Find( temp => temp.Equals(picker.ToItemInfo()) );

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
            return Items.Exists(temp => temp.Equals(item) );
        }

        public bool IsItemEquipped(ItemInfo item)
        {
            return EquippedItem != null && EquippedItem.Equals(item);
        }

        public bool IsItemUsed(ItemInfo item)
        {
            return DialogueManager.GetBoolVariable(item.UsedVariableName);
        }

        #region Events
        public event EventHandler<ItemEventArgs> ItemPicked;
        public event EventHandler<ItemEventArgs> ItemUsed;
        public event EventHandler<ItemEventArgs> ItemUpdated;
        public event EventHandler<ItemEventArgs> ItemEquipped;
        public event EventHandler ItemUnequipped;

        private void OnItemAdded(ItemInfo itemAdded)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemAdded);
            ItemPicked?.Invoke(this, itemEventArgs);
        }

        private void OnItemRemoved(ItemInfo itemRemoved)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemRemoved);
            ItemUsed?.Invoke(this, itemEventArgs);
        }

        private void OnItemUpdated(ItemInfo itemUpdated)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemUpdated);
            ItemUpdated?.Invoke(this, itemEventArgs);
        }

        private void OnItemEquipped(ItemInfo itemEquipped)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemEquipped);
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