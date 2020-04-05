using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class ItemUsePromptController : MonoBehaviour
    {

        public Inventory Inventory;

        public Canvas ItemUsePromptCanvas;
        private bool IsOpen => ItemUsePromptCanvas.enabled && UsableItem != null;

        private ItemInfo UsableItem;

        void Start()
        {
            
        }

        void Update()
        {
            if(IsOpen && Input.GetKey(Inventory.GetInteractKey()))
            {
                Inventory.UseItem(UsableItem);
                Close();
            }
        }

        public void Open(ItemInfo item)
        {
            UsableItem = item;
            ItemUsePromptCanvas.enabled = true;
        }

        public void Close()
        {
            UsableItem = null;
            ItemUsePromptCanvas.enabled = false;
        }

    }
}