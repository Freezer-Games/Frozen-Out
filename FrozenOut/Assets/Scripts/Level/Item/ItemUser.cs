using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemUser : MonoBehaviour
    {
        public string ItemVariableName;
        public string UseAnimation;

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

        public ItemInfo ToItemInfo()
        {
            return new ItemInfo() { VariableName = this.ItemVariableName };
        }

    }
}