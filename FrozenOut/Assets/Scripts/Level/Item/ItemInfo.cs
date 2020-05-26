using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Level.Item
{

    [Serializable]
    public class ItemPickerInfo : ItemBase
    {
        public int Quantity = 0;
    }

    [Serializable]
    public class ItemUserInfo : ItemBase
    {
    }

    [Serializable]
    public class ItemEquipperInfo : ItemBase
    {
    }

    [Serializable]
    public class ItemInfo : ItemBase
    {
        public string Name = "";
        public string Description = "";
        public bool IsEquippable = false;
        public int Quantity = 0;
        public bool IsUsed = false;
        public Sprite Sprite;
        public Sprite EquippedSprite;
    }

    [Serializable]
    public class ItemBase
    {
        public string VariableName;

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is ItemBase))
            {
                return false;
            }

            ItemBase other = (ItemBase)obj;
            return this.VariableName.Equals(other.VariableName);
        }
    }
}