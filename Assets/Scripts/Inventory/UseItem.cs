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
                    if (PlayerPrefs.GetString("Language") == "Es") { promptText.text = "Pulsa [" + PlayerPrefs.GetString("InteractKey", "F") + "] para utilizar"; }
                    else { promptText.text = "Press [" + PlayerPrefs.GetString("InteractKey", "F") + "] to use"; }
                }
                else if(!inventory.IsItemUsed(usableItemInfo))
                {
                    promptText.text = "Need to get <b>" + usableItemInfo.itemName + "</b>";
                    if (PlayerPrefs.GetString("Language") == "Es") { promptText.text = "necesitas conseguir " + usableItemInfo.itemName + " </ b >"; }
                    else { promptText.text = "Need to get <b>" + usableItemInfo.itemName + "</b>"; }
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