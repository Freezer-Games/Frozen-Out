using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    [RequireComponent(typeof(ItemInfo))]
    public class TriggerPickItem : MonoBehaviour
    {   
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        private string PlayerTag = "Player";
        private ItemInfo ItemInfo;

        void Start()
        {
            ItemInfo = GetComponent<ItemInfo>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                ItemInfo.OnPlayerClose();

                Inventory.OpenPickPrompt(ItemInfo);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(PlayerTag))
            {
                ItemInfo.OnPlayerAway();

                Inventory.ClosePickPrompt();
            }
        }

    }
}