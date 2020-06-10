using UnityEngine;

namespace Scripts.Level
{
    public class PlayerFollower : MonoBehaviour
    {
        public Transform Player;
        private readonly float HeightOffset = 10.0f;

        private void Update()
        {
            FollowPlayer(Player);
        }

        private void FollowPlayer(Transform playerTransform)
        {
            this.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + HeightOffset, playerTransform.position.z);
        }
    }
}