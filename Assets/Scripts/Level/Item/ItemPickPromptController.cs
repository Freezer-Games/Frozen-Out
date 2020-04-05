using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class ItemPickPromptController : MonoBehaviour
    {

        public Inventory Inventory;

        public Canvas ItemPickPromptCanvas;
        private bool IsOpen => ItemPickPromptCanvas.enabled && PickableItem != null;

        private ItemInfo PickableItem;

        void Start()
        {
            Close();
        }

        void Update()
        {
            if(IsOpen && Input.GetKey(Inventory.GetInteractKey()))
            {
                Inventory.PickItem(PickableItem);
                Close();
            }
        }

        public void Open(ItemInfo item)
        {
            PickableItem = item;
            ItemPickPromptCanvas.enabled = true;
        }

        public void Close()
        {
            PickableItem = null;
            ItemPickPromptCanvas.enabled = false;
        }

    }
}