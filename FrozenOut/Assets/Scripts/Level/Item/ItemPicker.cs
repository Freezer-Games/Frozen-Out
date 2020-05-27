using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemPicker : MonoBehaviour
    {
        public ItemPickerInfo Item;

        public void OnPickup()
        {
            gameObject.SetActive(false);
            //TODO
        }

        public void OnPlayerClose()
        {
            //TODO glow?
        }

        public void OnPlayerAway()
        {
            //TODO unglow?
        }
    }
}