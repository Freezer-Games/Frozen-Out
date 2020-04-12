using System;
using UnityEngine;

using Scripts.Settings;

using Scripts.Level.Sound;

namespace Scripts.Level.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerManager : MonoBehaviour
    {
        public LevelManager LevelManager;

        public PlayerController PlayerController;
        
        public GameObject Player;
        public bool InCinematic
        {
            get;
            private set;
        } = false;
        public bool IsEnabled
        {
            get;
            private set;
        }
        public bool IsGrounded => PlayerController.IsGrounded;
        public bool IsMoving => PlayerController.IsMoving;

        private Animator Animator;
        
        private SoundManager SoundManager => LevelManager.GetSoundManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();
        public event EventHandler<PlayerControllerEventArgs> Moving; 
        public event EventHandler Idle;
        
        void Start()
        {
            Animator = Player.GetComponent<Animator>();
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
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
            PlayerController.enabled = false;
        }

        public void EnableController()
        {
            Animator.enabled = true;
            PlayerController.enabled = true;
        }

        public void ToCheckPoint(Transform transform)
        {
            Instantiate(Player, transform.position, transform.rotation);
        }

        public void OnMoving()
        {
            PlayerControllerEventArgs controllerEvent = new PlayerControllerEventArgs();
            Moving?.Invoke(this, controllerEvent);
        }

        public void OnIdle()
        {
            Idle?.Invoke(this, EventArgs.Empty);
        }
    }
    

    [Serializable]
    public class PlayerControllerEventArgs : EventArgs
    {
        public bool Cancel
        {
            get;
            set;
        }
    }
}