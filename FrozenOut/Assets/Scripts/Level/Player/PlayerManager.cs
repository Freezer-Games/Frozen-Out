using System;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Settings;

using Scripts.Level.Sound;
using Scripts.Level.Item;

namespace Scripts.Level.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public LevelManager LevelManager;

        public PlayerController PlayerController;
        public GameObject Player;
        public List<ItemEquipper> EquippableObjects;
        public Animator Animator => PlayerController.GetAnimator();

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
        
        private SoundManager SoundManager => LevelManager.GetSoundManager();
        private SettingsManager SettingsManager => LevelManager.GetSettingsManager();

        private string PickAnimationName = "isPicking";

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
            SoundManager.Steps.Stop();
            PlayerController.enabled = false;
        }

        public void EnableController()
        {
            PlayerController.enabled = true;
        }

        public void ToCheckPoint(Transform transform)
        {
            Instantiate(Player, transform.position, transform.rotation);
        }

        public void EquipItem(ItemInfo itemEquipped)
        {
            foreach(ItemEquipper equipper in EquippableObjects)
            {
                if(itemEquipped.Equals(equipper.ToItemInfo()))
                {
                    PickAnimation();
                    equipper.OnEquip();
                }
            }
        }

        public void UnequipItem()
        {
            PickAnimation();
            foreach (ItemEquipper equipper in EquippableObjects)
            {
                equipper.OnUnequip();
            }
        }

        public void PickAnimation()
        {
            Animator.SetTrigger(PickAnimationName);
        }

        #region Events
        public event EventHandler<PlayerControllerEventArgs> Moving;
        public event EventHandler Idle;

        public void OnMoving()
        {
            PlayerControllerEventArgs controllerEvent = new PlayerControllerEventArgs();
            Moving?.Invoke(this, controllerEvent);
        }

        public void OnIdle()
        {
            Idle?.Invoke(this, EventArgs.Empty);
        }
        #endregion
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