using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemPicker : MonoBehaviour
    {
        public ItemPickerInfo Item;
        public Renderer Renderer;

        public void OnPickup()
        {
            gameObject.SetActive(false);
            //TODO
        }

        public void OnPlayerClose()
        {
            HighlightItem(true);
        }

        public void OnPlayerAway()
        {
            HighlightItem(false);
        }

        void HighlightItem(bool state)
        {
            Renderer.material.SetFloat("_Selected", state ? 1f : 0f);
        }
    }
}