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
            PlayerManager.SetInteractiveItem(user.GetItemPos(), user.GetItemLook());
        }

        public void CloseUsePrompt()
        {
            ItemUsePromptController.Close();
            if (!PlayerManager.GetIsInteracting())
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
            // Si no es necesario un item para usarlo, entonces se usa directamente
            if (string.IsNullOrEmpty(user.ItemVariableName))
            {
                PlayerManager.SetIsInteracting(true);
                StartCoroutine(WaitingPlayer(user));
                
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
                        UseEquippedItem(inventoryItem);
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

                OnItemUpdated(inventoryItem);
            }
        }

        private void UseConsumableItem(ItemInfo consumableItem)
        {
            Items.Remove(consumableItem);

            OnItemUsed(consumableItem);
            OnItemRemoved(consumableItem);
        }

        private void UseEquippedItem(ItemInfo equippedItem)
        {
            OnItemUsed(equippedItem);
        }

        public bool IsItemInInventory(ItemInfo item)
        {
            return Items.Exists(temp => temp.Equals(item) );
        }

        public bool IsItemEquipped(ItemInfo item)
        {
            return EquippedItem != null && EquippedItem.Equals(item);
        }

        IEnumerator WaitingPlayer(ItemUser user)
        {
            while (PlayerManager.GetIsInteracting())
            {
                yield return null;
            }
            user.OnUse();

            yield return null;
        }

        #region Events
        public event EventHandler<ItemEventArgs> ItemPicked;
        public event EventHandler<ItemEventArgs> ItemRemoved;
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
            ItemRemoved?.Invoke(this, itemEventArgs);
        }

        private void OnItemUpdated(ItemInfo itemUpdated)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemUpdated);
            ItemUpdated?.Invoke(this, itemEventArgs);
        }

        private void OnItemUsed(ItemInfo itemUsed)
        {
            ItemEventArgs itemEventArgs = new ItemEventArgs(itemUsed);
            ItemUsed?.Invoke(this, itemEventArgs);
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