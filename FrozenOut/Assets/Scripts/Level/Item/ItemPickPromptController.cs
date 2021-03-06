using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Level.Item
{
    public class ItemPickPromptController : UIController<ItemPicker>
    {
        public Inventory Inventory;

        public override bool IsOpen => Inventory.IsEnabled() && base.IsOpen && CandidatePicker != null;
        protected ItemPicker CandidatePicker;

        void Start()
        {
            Close();
        }

        void Update()
        {
            if(IsOpen && Input.GetKeyDown(Inventory.GetInteractKey()))
            {
                Inventory.PickItem(CandidatePicker);
                Close();
            }
        }
        
        public override void Open()
        {
            Open(null);
        }
        
        public override void Open(ItemPicker passingItem)
        {
            base.Open();

            CandidatePicker = passingItem;
        }

        public override void Close()
        {
            CandidatePicker = null;

            base.Close();
        }
    }
}