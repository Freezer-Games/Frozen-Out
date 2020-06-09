using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    [RequireComponent(typeof(ItemPicker))]
    public class TriggerPickItem : TriggerBase
    {   
        private Inventory Inventory => GameManager.CurrentLevelManager.GetInventory();

        private ItemPicker ItemPicker;

        void Start()
        {
            ItemPicker = GetComponent<ItemPicker>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                ItemPicker.OnPlayerClose();

                Inventory.OpenPickPrompt(ItemPicker);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                ItemPicker.OnPlayerAway();

                Inventory.ClosePickPrompt();
            }
        }

    }
}