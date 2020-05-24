using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public abstract class ItemUser : MonoBehaviour
    {
        public string ItemVariableName;
        public Transform InteractionPoint;

        public abstract void OnUse();

        public abstract void OnPlayerClose();

        public abstract void OnPlayerAway();

        public abstract void OnPlayerCol();

        public abstract void OnPlayerExitCol();

        public Transform GetInteractionPoint()
        {
            return InteractionPoint;
        }

        public ItemInfo ToItemInfo()
        {
            return new ItemInfo() { VariableName = this.ItemVariableName };
        }
    }
}