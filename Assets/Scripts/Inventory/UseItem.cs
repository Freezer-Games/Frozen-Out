using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Dialogue;

namespace Assets.Scripts.Item
{
    public class UseItem : MonoBehaviour
    {
        public KeyCode itemInput = KeyCode.F;
        public GameObject usableItem;
        public Text promptText;

        private ItemInfo usableItemInfo;
        private Inventory inventory;

        void Start()
        {
            inventory = FindObjectOfType<Inventory>();

            usableItemInfo = usableItem.GetComponent<ItemInfo>();
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                promptText.text = "";
                if(inventory.IsItemInInventory(usableItemInfo))
                {
                    promptText.text = "Press [" + itemInput.ToString() + "] to use <b>" + usableItemInfo.itemName + "</b>";
                }
                else if(!inventory.IsItemUsed(usableItemInfo))
                {
                    promptText.text = "Need to get <b>" + usableItemInfo.itemName + "</b>";
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player") && inventory.IsItemInInventory(usableItemInfo) && Input.GetButtonDown("Use"))
            {
                promptText.text = "";
                inventory.UseInventoryItem(usableItemInfo);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                promptText.text = "";
            }
        }
    }
}