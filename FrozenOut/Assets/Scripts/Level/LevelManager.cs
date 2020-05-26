using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Settings;

using Scripts.Level.Sound;
using Scripts.Level.Camera;
using Scripts.Level.Player;
using Scripts.Level.Dialogue;
using Scripts.Level.Mission;
using Scripts.Level.NPC;
using Scripts.Level.Item;

namespace Scripts.Level
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {

        public PlayerManager PlayerManager;
        public DialogueManager DialogueManager;
        public SoundManager SoundManager;
        //public CameraManager CameraManager;
        public Inventory Inventory;
        public NPCInfo[] NPCs;
        public MissionInfo[] Missions;

        private GameManager GameManager => GameManager.Instance;
        private SettingsManager SettingsManager => GameManager.SettingsManager;

        public void Load()
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            AudioListener.volume = Mathf.Clamp(SettingsManager.MusicVolume / 100f, 0, 1);

            if (Inventory != null)
            {
                Inventory.CloseMenu();
                Inventory.CloseUsePrompt();
            }

            if (PlayerManager != null)
            {
                PlayerManager.Enable();
            }

            #region EventBinding
            if (DialogueManager != null && SoundManager != null)
            {
                DialogueManager.Started += (sender, args) => SoundManager.DecreaseVolume();
                DialogueManager.Ended += (sender, args) => SoundManager.IncreaseVolume();
            }

            if (DialogueManager != null && PlayerManager != null)
            {
                DialogueManager.Started += (sender, args) => PlayerManager.Disable();
                DialogueManager.Ended += (sender, args) => PlayerManager.Enable();
            }

            if (Inventory != null && PlayerManager != null)
            {
                Inventory.ItemPicked += (sender, args) => PlayerManager.PickAnimation();
                Inventory.ItemEquipped += (sender, args) => PlayerManager.EquipItem(args.Item);
                Inventory.ItemUnequipped += (sender, args) => PlayerManager.UnequipItem();
                //Inventory.ItemUsed +=
            }
            #endregion

            // TODO
        }

        public void Unload()
        {
            // TODO
        }

        public void EnablePauseMenu()
        {
            GameManager.PauseMenuManager.Enable();
        }

        public void DisablePauseMenu()
        {
            GameManager.PauseMenuManager.Disable();
        }

        public SettingsManager GetSettingsManager()
        {
            return SettingsManager;
        }

        public PlayerManager GetPlayerManager()
        {
            return PlayerManager;
        }

        public DialogueManager GetDialogueManager()
        {
            return DialogueManager;
        }

        public SoundManager GetSoundManager()
        {
            return SoundManager;
        }
        
        /*public CameraManager GetCameraManager()
        {
            return CameraManager;
        }*/

        public Inventory GetInventory()
        {
            return Inventory;
        }

        public IEnumerable<NPCInfo> GetNPCs()
        {
            return NPCs;
        }

        public IEnumerable<MissionInfo> GetMissions()
        {
            return Missions;
        }

    }
}