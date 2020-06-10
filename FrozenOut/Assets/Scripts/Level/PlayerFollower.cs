using Scripts.Level.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level
{
    public class PlayerFollower : MonoBehaviour
    {
        private PlayerManager PlayerManager => GameManager.Instance.CurrentLevelManager.GetPlayerManager();
        private readonly float HeightOffset = 10.0f;

        private void Update()
        {
            FollowPlayer(PlayerManager.Player.transform);
        }

        private void FollowPlayer(Transform playerTransform)
        {
            this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + HeightOffset, playerTransform.position.z);
        }
    }
}