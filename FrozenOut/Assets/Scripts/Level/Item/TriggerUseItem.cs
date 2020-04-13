using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level;

namespace Scripts.Level.Item
{
    public class TriggerUseItem : MonoBehaviour
    {   
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        private string PlayerTag = "Player";
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