using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemInfo : MonoBehaviour
    {
        public string itemName;
        public string variableName;
        public string usedVariableName
        {
            get{
                return "used_" + variableName;
            }
        }
        public bool isInitiallyInInventory = false;

        void Start()
        {
            bool startVisible = true;
            if(transform.parent.name == "Inventory")
            {
                startVisible = isInitiallyInInventory;
            }

            this.gameObject.SetActive(startVisible);
        }
    }
}