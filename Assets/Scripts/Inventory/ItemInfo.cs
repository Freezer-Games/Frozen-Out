using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemInfo : MonoBehaviour
    {
        public string itemName;
        public string variableName;
        public bool initialValue = false;
        public bool isWorldItem = false;

        void Start()
        {
            bool startVisible = true;
            if(!isWorldItem)
            {
                startVisible = initialValue;
            }

            this.gameObject.SetActive(startVisible);
        }
    }
}