using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public abstract class ItemUser : MonoBehaviour
    {
        public string ItemVariableName;
        //Punto al que ira el jugador para interactuar
        public Transform ItemPos;
        //Punto al que mirará el jugador al interacutar
        public Transform ItemLook;

        public abstract void OnUse();

        public abstract void OnPlayerClose();

        public abstract void OnPlayerAway();

        public abstract void OnPlayerCol();

        public abstract void OnPlayerExitCol();

        public Transform GetItemPos() { return ItemPos; }

        public Transform GetItemLook() { return ItemLook; }

        public ItemInfo ToItemInfo()
        {
            return new ItemInfo() { VariableName = this.ItemVariableName };
        }
    }
}