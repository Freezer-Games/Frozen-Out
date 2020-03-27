using System;
using UnityEngine;

using Scripts.Level.Sound;

namespace Scripts.Level.Player
{
    public class PlayerManager : MonoBehaviour
    {
        
        public GameObject Player;
        public bool InCinematic = false;

        private Animator Animator;
        private SoundManager SoundManager;
        private PlayerController Controller;
        
        void Start()
        {
            SoundManager = GameManager.Instance.CurrentLevelManager.GetSoundManager();
            
            Animator = Player.GetComponent<Animator>();
            Controller = Player.GetComponent<PlayerController>();
        }

        public void ToNormal()
        {
            InCinematic = false;
            EnableController();
        }

        public void ToCinematic()
        {
            InCinematic = true;
            DisableController();
        }

        public void DisableController()
        {
            Animator.enabled = false;
            SoundManager.Steps.Stop();
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