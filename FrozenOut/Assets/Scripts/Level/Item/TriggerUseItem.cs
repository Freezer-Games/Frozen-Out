using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level;

namespace Scripts.Level.Item
{
    public class TriggerUseItem : MonoBehaviour
    {   
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        void Start()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                ItemInfo targetItem = other.GetComponent<ItemInfo>();
                Inventory.OpenUsePrompt(targetItem);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Inventory.CloseUsePrompt();
            }
        }

    }
}