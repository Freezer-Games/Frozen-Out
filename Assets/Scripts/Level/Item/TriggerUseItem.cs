using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Scripts.Level;

namespace Scripts.Level.Item
{
    public class TriggerUseItem : MonoBehaviour
    {

        public ItemInfo UsableItem;
        
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        void Start()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Inventory.OpenUsePrompt(UsableItem);
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