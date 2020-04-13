using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class ItemPickPromptController : InventoryUIController
    {
        public Inventory Inventory;

        void Start()
        {
            Close();
        }

        void Update()
        {
            if(IsOpen && Input.GetKey(Inventory.GetInteractKey()))
            {
                Inventory.PickItem(StoredItem);
                Close();
            }
        }

    }
}