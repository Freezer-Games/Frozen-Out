using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class TriggerPickItem : MonoBehaviour
    {   
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        void Start()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Item"))
            {
                ItemInfo targetItem = other.GetComponent<ItemInfo>();
                Inventory.OpenPickPrompt(targetItem);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Item"))
            {
                Inventory.ClosePickPrompt();
            }
        }

    }
}