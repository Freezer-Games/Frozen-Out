using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemPicker : MonoBehaviour
    {
        public string ItemVariableName;
        public int ItemQuantity = 0;
        public string PickupAnimation;

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

        public ItemInfo ToItemInfo()
        {
            return new ItemInfo() { VariableName = this.ItemVariableName };
        }
    }
}