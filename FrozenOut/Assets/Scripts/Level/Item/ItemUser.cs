using System;
using UnityEngine;

namespace Scripts.Level.Item
{
    public abstract class ItemUser : MonoBehaviour
    {
        public ItemUserInfo Item;
        //Punto al que ira el jugador para interactuar
        public Transform ItemPos;
        //Punto al que mirará el jugador al interacutar
        public Transform ItemLook;

        public Renderer Renderer;

        public abstract void OnUse();

        public abstract void OnUnableUse();

        public virtual void OnPlayerClose()
        {
            HighlightItem(true);
        }

        public virtual void OnPlayerAway()
        {
            HighlightItem(false);
        }

        public abstract void OnPlayerCol();

        public abstract void OnPlayerExitCol();

        public void DestroyItem() { Destroy(gameObject); }

        protected void HighlightItem(bool state)
        {
            Renderer.material.SetFloat("_Selected", state ? 1f : 0f);
        }

        public Transform GetItemPos() { return ItemPos; }

        public Transform GetItemLook() { return ItemLook; }
    }
}