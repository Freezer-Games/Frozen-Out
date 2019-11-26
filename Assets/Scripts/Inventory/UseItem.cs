using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Dialogue;

namespace Assets.Scripts.Item
{
    public class UseItem : MonoBehaviour
    {
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
                    promptText.text = LocalizationManager.instance.GetLocalizedValue("Press") + PlayerPrefs.GetString("InteractKey", "F") + LocalizationManager.instance.GetLocalizedValue("use");
                }
                else if(!inventory.IsItemUsed(usableItemInfo))
                {
                    promptText.text = LocalizationManager.instance.GetLocalizedValue("need") + " " + usableItemInfo.itemName;
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player") && inventory.IsItemInInventory(usableItemInfo) && Input.GetKeyDown(GameManager.instance.interact))
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