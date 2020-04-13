using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    [Serializable]
    public class ItemInfo
    {
        public string Name = "";
        public string Description = "";
        public bool IsEquippable = false;
        public int Quantity = 0;
        public Sprite Sprite;
        public Sprite EquippedSprite;
        public string VariableName;
        public string UsedVariableName
        {
            get{
                return "used_" + VariableName;
            }
        }
        public string QuantityVariableName
        {
            get{
                return "quantity_" + VariableName;
            }
        }
    }
}