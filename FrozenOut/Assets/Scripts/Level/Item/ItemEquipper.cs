using UnityEngine;
using System.Collections;

namespace Scripts.Level.Item
{
    public class ItemEquipper : MonoBehaviour
    {
        public ItemEquipperInfo Item;

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