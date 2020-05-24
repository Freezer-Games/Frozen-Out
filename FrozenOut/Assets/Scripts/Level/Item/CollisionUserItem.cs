using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level;

namespace Scripts.Level.Item
{
    public class CollisionUserItem : MonoBehaviour
    {
        private Inventory Inventory => GameManager.Instance.CurrentLevelManager.GetInventory();

        private string PlayerTag = "Player";
        private ItemUser ItemUser;

        void Start()
        {
            ItemUser = GetComponent<ItemUser>();
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag(PlayerTag))
            {
                ItemUser.OnPlayerCol();
                Inventory.OpenUsePrompt(ItemUser);
            }
        }

        void OnCollisionExit(Collision other)
        {
            if (other.transform.CompareTag(PlayerTag))
            {
                ItemUser.OnPlayerExitCol();
                Inventory.CloseUsePrompt();
            }
        }
        
    }
}

