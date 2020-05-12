using UnityEngine;
using System.Collections;

namespace Scripts.Level.Item
{
    public class ItemEquipper : MonoBehaviour
    {
        public string ItemVariableName;
        public string EquipAnimation;

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

        public ItemInfo ToItemInfo()
        {
            return new ItemInfo() { VariableName = this.ItemVariableName };
        }
    }
}