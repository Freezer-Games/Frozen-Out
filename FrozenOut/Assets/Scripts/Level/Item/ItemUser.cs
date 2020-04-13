using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemUser : MonoBehaviour
    {
        public string ItemName = "";
        public string UseAnimation;

        void Start()
        {
            if(ItemName.Equals(""))
            {
                ItemName = gameObject.name;
            }
        }

        public void OnUse()
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