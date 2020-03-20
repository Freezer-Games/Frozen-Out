using System;
using UnityEngine;

namespace Scripts.Level.Player
{
    public class PlayerManager : MonoBehaviour
    {
        
        public bool inCinematic = false;

        private Animator Animator;
        private AudioManager AudioManager;
        private PlayerController Controller;
        private GameObject Player;
        
        void Start()
        {
            AudioManager = GameManager.Instance.CurrentLevelManager.GetAudioManager();
            
            Player = GameObject.Find("POL");
            Animator = player.GetComponent<Animator>();
            Controller = player.GetComponent<PlayerController>();
        }

        public void DisableController()
        {
            Animator.enabled = false;
            AudioManager.Steps.Stop();
            Controller.enabled = false;
        }

        public void EnableController()
        {
            Animator.enabled = true;
            Controller.enabled = true;
        }

        public void ToCheckPoint(Transform transform)
        {
            Instantiate(Player, transform.position, transform.rotation);
        }
    }
}