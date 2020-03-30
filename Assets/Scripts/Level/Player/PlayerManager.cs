using System;
using UnityEngine;

using Scripts.Settings;

using Scripts.Level.Sound;

namespace Scripts.Level.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public LevelManager LevelManager;
        
        public GameObject Player;
        public bool InCinematic = false;

        private Animator Animator;
        private PlayerController Controller;

        
        private SoundManager SoundManager => LevelManager.GetSoundManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();
        
        void Start()
        {   
            Animator = Player.GetComponent<Animator>();
            Controller = Player.GetComponent<PlayerController>();
        }

        public bool IsStepsPlaying()
        {
            return SoundManager.Steps.isPlaying;
        }

        public void PlaySteps()
        {
            SoundManager.Steps.Play();
        }

        public void StopSteps()
        {
            SoundManager.Steps.Stop();
        }

        public KeyCode GetJumpKey()
        {
            return SettingsManager.JumpKey;
        }

        public KeyCode GetCrouchKey()
        {
            return SettingsManager.CrouchKey;
        }

        public KeyCode [] GetMovementKeys()
        {
            return SettingsManager.MovementKeys;
        }

        public KeyCode GetForwardKey()
        {
            return SettingsManager.ForwardKey;
        }

        public KeyCode GetBackKey()
        {
            return SettingsManager.BackwardKey;
        }

        public KeyCode GetRightKey()
        {
            return SettingsManager.RightKey;
        }

        public KeyCode GetLeftKey()
        {
            return SettingsManager.LeftKey;
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