using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemUsePromptController : UIController<ItemUser>
    {
        public Inventory Inventory;

        public override bool IsOpen => Inventory.IsEnabled() && base.IsOpen && CandidateUser != null;
        protected ItemUser CandidateUser;

        void Update()
        {
            if(IsOpen && Input.GetKeyDown(Inventory.GetInteractKey()))
            {
                Inventory.UseItem(CandidateUser);
                Close();
            }
        }
        
        public override void Open()
        {
            Open(null);
        }
        
        public override void Open(ItemUser passingItem)
        {
            base.Open();

            CandidateUser = passingItem;
        }

        public override void Close()
        {
            CandidateUser = null;

            base.Close();
        }
    }
}