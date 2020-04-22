using UnityEngine;
using System.Collections;

namespace Scripts.Level.Item
{
    public class ItemEquipper : MonoBehaviour
    {
        public string ItemName = "";
        public string EquipAnimation;

        void Start()
        {
            if (ItemName.Equals(""))
            {
                ItemName = gameObject.name;
            }
        }

        public void OnEquip()
        {
            gameObject.SetActive(true);
            //TODO
        }

        public void OnUnequip()
        {
            gameObject.SetActive(false);
            //TODO
        }
    }
}