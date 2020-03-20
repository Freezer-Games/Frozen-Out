using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemInfo : MonoBehaviour
    {
        
        public string Name;
        public string VariableName;
        public string UsedVariableName
        {
            get{
                return "used_" + VariableName;
            }
        }
        public bool IsInitiallyInInventory = false;

        void Start()
        {
            bool startVisible = true;
            if(transform.parent.name == "Inventory")
            {
                startVisible = IsInitiallyInInventory;
            }

            this.gameObject.SetActive(startVisible);
        }

    }
}