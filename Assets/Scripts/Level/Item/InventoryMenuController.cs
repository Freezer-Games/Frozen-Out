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
        [SerializeField]
        private int SelectedItemIndex;

        void Start()
        {
            // Quitar cualquier elmento que se haya quedado
            foreach(Transform itemObject in ItemsGroup.transform)
            {
                Destroy(itemObject.gameObject);
            }

            Inventory.ItemAdded += (sender, args) => AddItemImage(args.Item);
            Inventory.ItemRemoved += (sender, args) => RemoveItemImage(args.Item);
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
                    EquipUnequipSelectedItem();
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
            ItemInfo selectedItem = Inventory.Items[SelectedItemIndex];
            if(selectedItem.IsEquippable)
            {
                Image selectedItemImage = GetItemImage(SelectedItemIndex);
                if(Inventory.IsItemEquipped(selectedItem))
                {
                    UnequipItem(selectedItemImage, selectedItem);
                }
                else
                {
                    EquipItem(selectedItemImage, selectedItem);
                }
                
            }
        }

        private void UnequipItem(Image selectedItemImage, ItemInfo selectedItem)
        {
            Inventory.UnequipItem();

            selectedItemImage.sprite = selectedItem.Sprite;
        }

        private void EquipItem(Image selectedItemImage, ItemInfo selectedItem)
        {
            if(Inventory.EquippedItem != null)
            {
                Image previousEquippedImage = GetItemImage(Inventory.Items.IndexOf(Inventory.EquippedItem));
                previousEquippedImage.sprite = Inventory.EquippedItem.Sprite;
            }
            Inventory.EquipItem(selectedItem);

            selectedItemImage.sprite = selectedItem.EquippedSprite;
        }

        private Image GetItemImage(int index)
        {
            Transform itemObject = ItemsGroup.transform.GetChild(index);
            Image itemImage = itemObject.GetComponent<Image>();

            return itemImage;
        }

        public void Open()
        {
            Inventory.LevelManager.DisablePauseMenu();
            InventoryMenuCanvas.enabled = true;
            Time.timeScale = 0;

            ChangeSelectedItem(0);
        }

        public void Close()
        {
            Inventory.LevelManager.EnablePauseMenu();
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