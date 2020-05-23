using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Level.Item
{
    public class InventoryMenuController : UIController
    {
        public Inventory Inventory;

        public Text NameText;
        public Text DescriptionText;

        public HorizontalLayoutGroup ItemsGroup;
        public GameObject ItemImagePrefab;
        public GameObject Arrow;
        
        private int SelectedItemIndex;
        private ItemInfo PendingEquippedItem;

        void Start()
        {
            // Quitar cualquier elmento que se haya quedado
            foreach(Transform itemObject in ItemsGroup.transform)
            {
                Destroy(itemObject.gameObject);
            }

            Inventory.ItemPicked += (sender, args) => AddItemImage(args.Item);
            Inventory.ItemUsed += (sender, args) => RemoveItemImage(args.Item);
            Inventory.ItemUpdated += (sender, args) => UpdateItemImage(args.Item);
        }

        void Update()
        {
            if(Input.GetKeyDown(Inventory.GetInventoryKey()))
            {
                CloseOpenMenu();
            }

            if(IsOpen)
            {
                if(SelectedItemIndex >= 0 && Input.GetKeyDown(Inventory.GetInteractKey()))
                {
                    UpdateEquipUnequipPending();
                }
                if(Input.GetKeyDown(KeyCode.RightArrow))
                {
                    ChangeSelectedItem(SelectedItemIndex + 1);
                }
                else if(Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    ChangeSelectedItem(SelectedItemIndex - 1);
                }
            }
        }

        public override void Open()
        {
            base.Open();

            Inventory.LevelManager.DisablePauseMenu();
            Time.timeScale = 0;

            ChangeSelectedItem(0);
        }

        public override void Close()
        {
            base.Close();

            Inventory.LevelManager.EnablePauseMenu();
            Time.timeScale = 1;

            EquipUnequipSelectedItem();
        }

        private void AddItemImage(ItemInfo itemAdded)
        {
            GameObject itemElement = GameObject.Instantiate(ItemImagePrefab, ItemsGroup.transform);
            Image itemImage = itemElement.GetComponent<Image>();
            itemImage.sprite = itemAdded.Sprite;
            if(Inventory.IsItemEquipped(itemAdded))
            {
                itemImage.sprite = itemAdded.EquippedSprite;
            }
            if(itemAdded.Quantity > 0)
            {
                UpdateImageQuantity(itemImage, itemAdded.Quantity);
            }
        }

        private void UpdateItemImage(ItemInfo itemUpdated)
        {
            Image itemImage = GetItemImage(Inventory.Items.IndexOf(itemUpdated));
            if(itemUpdated.Quantity > 0)
            {
                UpdateImageQuantity(itemImage, itemUpdated.Quantity);
            }
        }

        private void UpdateImageQuantity(Image itemImage, int newQuantity)
        {
            Text itemQuantityText = itemImage.GetComponentInChildren<Text>();
            itemQuantityText.enabled = true;
            itemQuantityText.text = "x" + newQuantity.ToString();
        }

        private void RemoveItemImage(ItemInfo itemRemoved)
        {
            Image itemImage = GetItemImage(Inventory.Items.IndexOf(itemRemoved));

            Destroy(itemImage.gameObject);
        }

        private void ChangeSelectedItem(int newIndex)
        {
            int clampedIndex = -1;
            if(Inventory.Items.Count > 0)
            {
                clampedIndex = Mathf.Clamp(newIndex, 0, Inventory.Items.Count - 1);
            }

            SelectedItemIndex = clampedIndex;
            PendingEquippedItem = Inventory.EquippedItem;
            UpdateTexts();
            UpdateArrow();
        }

        private void UpdateTexts()
        {
            NameText.text = "Inventario vacío";
            DescriptionText.text = "Ve a coger algun ítem";
            if(SelectedItemIndex >= 0)
            {
                ItemInfo selectedItem = Inventory.Items[SelectedItemIndex];
                NameText.text = selectedItem.Name;
                DescriptionText.text = selectedItem.Description;
            }
        }

        private void UpdateArrow()
        {
            Arrow.SetActive(false);
            if(SelectedItemIndex >= 0)
            {
                Arrow.SetActive(true);

                Image selectedItemImage = GetItemImage(SelectedItemIndex);
                Transform arrowPoint = selectedItemImage.transform.GetChild(0).transform;

                Transform arrowTransform = Arrow.transform;
                Vector3 newArrowPosition = arrowPoint.position;
                arrowTransform.position = newArrowPosition;
            }
        }

        private void EquipUnequipSelectedItem()
        {
            if (PendingEquippedItem == null)
            {
                Inventory.UnequipItem();
            }
            else
            {
                Inventory.EquipItem(PendingEquippedItem);
            }
        }

        private void UpdateEquipUnequipPending()
        {
            ItemInfo selectedItem = Inventory.Items[SelectedItemIndex];
            if (selectedItem.IsEquippable)
            {
                Image selectedItemImage = GetItemImage(SelectedItemIndex);
                if (IsItemPendingEquipped(selectedItem))
                {
                    UpdateUnequipPending(selectedItemImage, selectedItem);
                }
                else
                {
                    UpdateEquipPending(selectedItemImage, selectedItem);
                }
            }
        }

        private void UpdateUnequipPending(Image selectedItemImage, ItemInfo selectedItem)
        {
            selectedItemImage.sprite = selectedItem.Sprite;

            PendingEquippedItem = null;
        }

        private void UpdateEquipPending(Image selectedItemImage, ItemInfo selectedItem)
        {
            if (PendingEquippedItem != null)
            {
                Image previousEquippedImage = GetItemImage(Inventory.Items.IndexOf(PendingEquippedItem));
                previousEquippedImage.sprite = PendingEquippedItem.Sprite;
            }
            selectedItemImage.sprite = selectedItem.EquippedSprite;

            PendingEquippedItem = selectedItem;
        }

        private Image GetItemImage(int index)
        {
            Transform itemObject = ItemsGroup.transform.GetChild(index);
            Image itemImage = itemObject.GetComponent<Image>();

            return itemImage;
        }

        public bool IsItemPendingEquipped(ItemInfo item)
        {
            return PendingEquippedItem != null && PendingEquippedItem.Equals(item);
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