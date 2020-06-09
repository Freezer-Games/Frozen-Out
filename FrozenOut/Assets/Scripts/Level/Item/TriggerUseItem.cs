using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level;

namespace Scripts.Level.Item
{
    public class TriggerUseItem : TriggerBase
    {   
        private Inventory Inventory => GameManager.CurrentLevelManager.GetInventory();

        private ItemUser ItemUser;

        void Start()
        {
            ItemUser = GetComponent<ItemUser>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                ItemUser.OnPlayerClose();

                Inventory.OpenUsePrompt(ItemUser);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                ItemUser.OnPlayerAway();

                Inventory.CloseUsePrompt();
            }
        }

    }
}