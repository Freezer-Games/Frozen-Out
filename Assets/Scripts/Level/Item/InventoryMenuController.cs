using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class InventoryMenuController : MonoBehaviour
    {

        public Inventory Inventory;

        public Canvas InventoryMenuCanvas;
        private bool IsOpen => InventoryMenuCanvas.enabled;

        void Start()
        {
            
        }

        public void Open()
        {
            InventoryMenuCanvas.enabled = true;
        }

        public void Close()
        {
            InventoryMenuCanvas.enabled = false;
        }

    }
}