using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemUsePromptController : InventoryUIController
    {
        public Inventory Inventory;

        void Update()
        {
            if(IsOpen && Input.GetKey(Inventory.GetInteractKey()))
            {
                Inventory.UseItem(StoredItem);
                Close();
            }
        }
    }
}