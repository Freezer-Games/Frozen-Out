using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class InventoryMenuController : MonoBehaviour
    {
        public Inventory Inventory;

        public Canvas InventoryMenuCanvas;
        private bool IsOpen => InventoryMenuCanvas.enabled;

        public Text NameText;
        public Text DescriptionText;

        public HorizontalLayoutGroup ItemsGroup;
        public GameObject ItemImagePrefab;
        public GameObject Arrow;

        private ItemInfo SelectedItem;
        private int SelectedItemIndex;

        void Start()
        {

        }

        void Update()
        {
            if(Input.GetKeyDown(Inventory.GetInventoryKey()))
            {
                CloseOpenMenu();
            }

            if(IsOpen && SelectedItem != null && Input.GetKeyDown(Inventory.GetInteractKey()))
            {
                EquipSelectedItem();
            }
        }

        private void LoadItems()
        {
            foreach(Transform itemObject in ItemsGroup.transform)
            {
                Destroy(itemObject.gameObject);
            }
            foreach(ItemInfo item in Inventory.Items)
            {
                GameObject itemElement = GameObject.Instantiate(ItemImagePrefab, ItemsGroup.transform);
                Image itemImage = itemElement.GetComponent<Image>();
                itemImage.sprite = item.Sprite;
                if(Inventory.IsItemEquipped(item))
                {
                    itemImage.sprite = item.EquippedSprite;
                }
            }
        }

        private void ChangeSelectedItem(ItemInfo newSelection)
        {
            SelectedItem = newSelection;
            SelectedItemIndex = Inventory.Items.IndexOf(newSelection);
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            NameText.text = "Inventario vacío";
            DescriptionText.text = "Ve a coger algun ítem";
            if(SelectedItem != null)
            {
                NameText.text = SelectedItem.Name;
                DescriptionText.text = SelectedItem.Description;
            }
        }

        private void EquipSelectedItem()
        {
            bool equipped = Inventory.EquipItem(SelectedItem);
            Transform itemObjectEquipped = ItemsGroup.transform.GetChild(SelectedItemIndex);
            if(equipped)
            {
                itemObjectEquipped.GetComponent<Image>().sprite = SelectedItem.EquippedSprite;
            }
            else
            {
                itemObjectEquipped.GetComponent<Image>().sprite = SelectedItem.Sprite;
            }
        }

        public void Open()
        {
            InventoryMenuCanvas.enabled = true;
            Time.timeScale = 0;

            LoadItems();
            ChangeSelectedItem(Inventory.Items.Count > 0? Inventory.Items[0] : null);
        }

        public void Close()
        {
            InventoryMenuCanvas.enabled = false;
            Time.timeScale = 1;
            //TODO activate animation of equipped
        }

        private void CloseOpenMenu()
        {
            if(IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

    }
}