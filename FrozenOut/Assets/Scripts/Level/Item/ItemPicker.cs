using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemPicker : MonoBehaviour
    {
        public string ItemName = "";
        public int ItemQuantity = 0;
        public string PickupAnimation;

        void Start()
        {
            if(ItemName.Equals(""))
            {
                ItemName = gameObject.name;
            }
        }

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