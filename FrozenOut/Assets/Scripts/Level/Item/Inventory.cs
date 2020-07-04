using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Scripts.Settings;
using Scripts.Level.Player;
using Scripts.Level.Sound;

namespace Scripts.Level.Item
{
    public class Inventory : BaseManager
    {
        public LevelManager LevelManager;
        
        public UIController InventoryMenuController;
        public ItemPickPromptController ItemPickPromptController;
        public ItemUsePromptController ItemUsePromptController;

        [Header("Sound")]
        public InventorySoundController SoundController;

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
            if(IsEnabled())
            {
                InventoryMenuController.Open();
            }
        }

        public void CloseMenu()
        {
            InventoryMenuController.Close();
        }

        public void OpenUsePrompt(ItemUser user)
        {
            if (IsEnabled())
            {
                ItemUsePromptController.Open(user);
                PlayerManager.SetInteractiveItem(user.GetItemPos(), user.GetItemLook());
            }   
        }

        public void CloseUsePrompt()
        {
            ItemUsePromptController.Close();
            if (!PlayerManager.GetIsInteracting())
            {
                PlayerManager.SetInteractiveItem(null, null);
            }
        }

        public void OpenPickPrompt(ItemPicker picker)
        {
            if (IsEnabled())
            {
                ItemPickPromptController.Open(picker);
            }
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
            if (IsEnabled())
            {
                if (IsItemInInventory(item) && item.IsEquippable)
                {
                    EquippedItem = item;
                    PlayerManager.SetInteractAnimation(EquippedItem.Animation);

                    OnItemEquipped(item);
                }
            }
        }

        /// Desequipa el item equipado
        public void UnequipItem()
        {
            if (IsEnabled())
            {
                EquippedItem = null;
                PlayerManager.SetInteractAnimation("");

                OnItemUnequipped();
            }
        }

        public void PickItem(ItemPickerInfo pickerInfo)
        {
            if (IsEnabled())
            {
                if (IsItemInInventory(pickerInfo))
                {
                    UpdateItem(pickerInfo);
                }
                else
                {
                    AddItem(pickerInfo);
                }
            }
        }
        public void PickItem(ItemPicker picker)
        {
            picker.OnPickup();

            PickItem(picker.Item);
        }


        public void UseItem(ItemUserInfo userInfo)
        {
            if (IsEnabled())
            {
                if (IsItemInInventory(userInfo))
                {
                    //Pasarlo a ItemInfo del inventario
                    ItemInfo inventoryItem = Items.Find(temp => temp.Equals(userInfo));

                    if (inventoryItem.IsEquippable)
                    {
                        if (IsItemEquipped(inventoryItem))
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
        }
        public void UseItem(ItemUser user)
        {
            // Si no es necesario un item para usarlo, entonces se usa directamente
            if (string.IsNullOrEmpty(user.Item.VariableName))
            {
                PlayerManager.SetIsInteracting(true);
                StartCoroutine(WaitingPlayer(user, false));
            }
            else if(IsItemInInventory(user.Item))
            {
                //Pasarlo a ItemInfo del inventario
                ItemInfo inventoryItem = Items.Find(temp => temp.Equals(user.Item));

                if (inventoryItem.IsEquippable)
                {
                    if (IsItemEquipped(inventoryItem))
                    {
                        PlayerManager.SetIsInteracting(true);
                        StartCoroutine(WaitingPlayer(user, true));
                    }
                    else
                    {
                        user.OnUnableUse();
                    }
                }

                UseItem(user.Item);
            }
            else
            {
                user.OnUnableUse();
            }
        }

        private void AddItem(ItemPickerInfo pickerInfo)
        {
            //Pasarlo a ItemInfo del nivel
            ItemInfo item = LevelItems.Find(temp => temp.Equals(pickerInfo));
            item.Quantity = pickerInfo.Quantity;

            Items.Add(item);

            OnItemAdded(item);
        }

        private void UpdateItem(ItemPickerInfo pickerInfo)
        {
            if (pickerInfo.Quantity > 0)
            {
                //Pasarlo a ItemInfo del inventario
                ItemInfo inventoryItem = Items.Find(temp => temp.Equals(pickerInfo));

                int currentQuantity = inventoryItem.Quantity;
                int newQuantity = currentQuantity + pickerInfo.Quantity;

                inventoryItem.Quantity = newQuantity;

                OnItemUpdated(inventoryItem);
            }
        }

        private void UseConsumableItem(ItemInfo consumableItem)
        {
            consumableItem.IsUsed = true;

            OnItemUsed(consumableItem);
            OnItemRemoved(consumableItem);
        }

        private void UseEquippedItem(ItemInfo equippedItem)
        {
            equippedItem.IsUsed = true;

            OnItemUsed(equippedItem);
        }

        public bool IsItemInInventory(ItemBase item)
        {
            return Items.Exists(temp => temp.Equals(item));
        }

        public bool IsItemEquipped(ItemBase item)
        {
            return EquippedItem != null && EquippedItem.Equals(item);
        }

        public bool IsItemUsed(ItemBase item)
        {
            if(IsItemInInventory(item))
            {
                ItemInfo inventoryItem = this.Items.Find(temp => temp.Equals(item));

                return inventoryItem.IsUsed;
            }
            else
            {
                return false;
            }
        }
        
        public int QuantityOfItem(ItemBase item)
        {
            if(IsItemInInventory(item))
            {
                ItemInfo inventoryItem = this.Items.Find(temp => temp.Equals(item));

                return inventoryItem.Quantity;
            }
            else
            {
                return 0;
            }
        }

        IEnumerator WaitingPlayer(ItemUser user, bool needTool)
        {
            while (PlayerManager.GetIsInteracting())
            {
                yield return null;
            }

            if (!needTool)
                PlayerManager.PickAnimation();
            else
                PlayerManager.PlayInteractAnimation();
                
            user.OnUse();
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