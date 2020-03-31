using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class InventoryUseMenuController : MonoBehaviour
    {

        public Inventory Inventory;

        public Canvas InventoryUseMenuCanvas;
        private bool IsOpen => InventoryUseMenuCanvas.enabled && UsableItem != null;

        private ItemInfo UsableItem;

        void Start()
        {
            
        }

        void Update()
        {
            if(IsOpen && !Inventory.IsItemInInventory(UsableItem) && Input.GetKey(Inventory.GetInteractKey()))
            {
                Inventory.UseInventoryItem(UsableItem);
                UsableItem = null;
            }
        }

        public void Open(ItemInfo usableItem)
        {
            UsableItem = usableItem;
            InventoryUseMenuCanvas.enabled = true;
        }

        public void Close()
        {
            UsableItem = null;
            InventoryUseMenuCanvas.enabled = false;
        }

        private void UseItem()
        {
            Inventory.UseInventoryItem(UsableItem);
            Close();
        }

    }
}