using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Item
{
    public class ItemImage : MonoBehaviour
    {
        public ItemBase Item;

        public Image SpriteImage;
        public Transform ArrowPoint;
        public Text QuantityText;

        public void SetItem(ItemBase originalItem)
        {
            Item.VariableName = originalItem.VariableName;
        }

        public void SetSprite(Sprite sprite)
        {
            this.SpriteImage.sprite = sprite;
        }

        public void SetQuantityText(int quantity)
        {
            if (quantity > 0)
            {
                QuantityText.enabled = true;
                QuantityText.text = "x" + quantity.ToString();
            }
        }
    }
}