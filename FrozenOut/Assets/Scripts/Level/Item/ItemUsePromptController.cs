using System.Collections;
using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemUsePromptController : UIController<ItemUser>
    {
        public Inventory Inventory;

        protected override bool IsOpen => base.IsOpen && CandidateUser != null;
        protected ItemUser CandidateUser;

        void Update()
        {
            if(IsOpen && Input.GetKey(Inventory.GetInteractKey()))
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