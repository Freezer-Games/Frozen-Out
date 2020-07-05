using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Scripts.Level.Item
{
    public class InventoryMenuController : UIController
    {
        public Inventory Inventory;

        public GameObject NameObject;
        public GameObject DescriptionObject;

        public HorizontalLayoutGroup ItemsGroup;
        public GameObject ItemImagePrefab;
        public GameObject Arrow;
        
        private int SelectedItemIndex;
        private ItemInfo PendingEquippedItem;
        private List<ItemImage> ItemImages;

        private LocalizedString LocalizeScriptName => NameObject.GetComponent<LocalizeStringBehaviour>().StringReference;
        private LocalizedString LocalizeScriptDescription => DescriptionObject.GetComponent<LocalizeStringBehaviour>().StringReference;

        void Start()
        {
            // Quitar cualquier ítem que se haya quedado
            foreach(Transform itemObject in ItemsGroup.transform)
            {
                Destroy(itemObject.gameObject);
            }
            ItemImages = new List<ItemImage>();

            Inventory.ItemPicked += (sender, args) => AddItemImage(args.Item);
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
            GameObject itemObject = GameObject.Instantiate(ItemImagePrefab, ItemsGroup.transform);
            ItemImage itemImage = itemObject.GetComponent<ItemImage>();

            itemImage.SetItem(itemAdded);
            itemImage.SetSprite(itemAdded.Sprite);
            itemImage.SetQuantityText(itemAdded.Quantity);

            ItemImages.Add(itemImage);
        }

        private void UpdateItemImage(ItemInfo itemUpdated)
        {
            ItemImage itemImage = GetItemImage(itemUpdated);

            itemImage.SetQuantityText(itemUpdated.Quantity);
        }

        private void RemoveItemImage(ItemInfo itemRemoved)
        {
            ItemImage itemImage = GetItemImage(itemRemoved);

            ItemImages.Remove(itemImage);
            Destroy(itemImage.gameObject);
        }

        private void ChangeSelectedItem(int newIndex)
        {
            int clampedIndex = -1;
            if(ItemImages.Count > 0)
            {
                clampedIndex = Mathf.Clamp(newIndex, 0, ItemImages.Count - 1);

                Inventory.SoundController.PlayClip(Inventory.SoundController.Pasar);
            }

            SelectedItemIndex = clampedIndex;
            UpdateTexts();
            UpdateArrow();
        }

        private void UpdateTexts()
        {
            if(SelectedItemIndex >= 0)
            {
                ItemImage selectedItemImage = GetItemImage(SelectedItemIndex);
                ItemBase selectedItem = selectedItemImage.Item;

                LocalizeScriptName.TableEntryReference = selectedItem.VariableName;
                LocalizeScriptDescription.TableEntryReference = selectedItem.VariableName;
            }
            else
            {
                LocalizeScriptName.TableEntryReference = "empty";
                LocalizeScriptDescription.TableEntryReference = "empty";
            }
        }

        private void UpdateArrow()
        {
            Arrow.SetActive(false);
            if(SelectedItemIndex >= 0)
            {
                Arrow.SetActive(true);

                ItemImage selectedItemImage = GetItemImage(SelectedItemIndex);
                Transform arrowPoint = selectedItemImage.ArrowPoint;

                Vector3 newArrowPosition = arrowPoint.position;
                Arrow.transform.position = newArrowPosition;
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
            ItemImage selectedItemImage = GetItemImage(SelectedItemIndex);
            ItemInfo selectedItem = Inventory.GetInventoryItem(selectedItemImage.Item);

            if (selectedItem.IsEquippable)
            {
                Inventory.SoundController.PlayClip(Inventory.SoundController.Seleccion);

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

        private void UpdateUnequipPending(ItemImage selectedItemImage, ItemInfo selectedItem)
        {
            selectedItemImage.SetSprite(selectedItem.Sprite);

            PendingEquippedItem = null;
        }

        private void UpdateEquipPending(ItemImage selectedItemImage, ItemInfo selectedItem)
        {
            if (PendingEquippedItem != null)
            {
                ItemImage previousEquippedImage = GetItemImage(PendingEquippedItem);
                previousEquippedImage.SetSprite(PendingEquippedItem.Sprite);
            }
            selectedItemImage.SetSprite(selectedItem.EquippedSprite);

            PendingEquippedItem = selectedItem;
        }

        private ItemImage GetItemImage(ItemBase itemInfo)
        {
            ItemImage itemImage = ItemImages.Find(temp => temp.Item.Equals(itemInfo));

            return itemImage;
        }

        private ItemImage GetItemImage(int index)
        {
            ItemImage itemImage = ItemImages.ElementAt(index);

            return itemImage;
        }

        public bool IsItemPendingEquipped(ItemBase item)
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