using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Item
{
    public class ItemInfo : MonoBehaviour
    {
        
        public string Name = "";
        public string Description = "";
        public bool IsEquippable = false;
        public int Quantity = 0;
        public Sprite Sprite;
        public Sprite EquippedSprite;
        public string PickupAnimation;
        public string UseAnimation;
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

        public void OnPickup()
        {
            gameObject.SetActive(false);
            //TODO
        }

        public void OnPlayerClose()
        {
            //TODO glow?
        }

        public void OnPlayerAway()
        {
            //TODO
        }

    }
}