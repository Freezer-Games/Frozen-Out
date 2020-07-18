using UnityEngine;

using Scripts.Settings;

using Scripts.Level.Sound;
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
        public MusicManager MusicManager;
        public Inventory Inventory;
        public NPCManager NPCManager;
        public MissionManager MissionManager;

        private GameManager GameManager => GameManager.Instance;
        private SettingsManager SettingsManager => GameManager.SettingsManager;

        public void Load()
        {
            Enable();
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            AudioListener.volume = Mathf.Clamp(SettingsManager.MusicVolume / 100f, 0, 1);

            if (Inventory != null)
            {
                Inventory.CloseMenu();
                Inventory.CloseUsePrompt();
            }

            #region EventBinding
            if (DialogueManager != null && MusicManager != null)
            {
                DialogueManager.Started += (sender, args) => MusicManager.DecreaseVolume();
                DialogueManager.Ended += (sender, args) => MusicManager.IncreaseVolume();
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

        public void Enable()
        {
            PlayerManager.Enable();
            DialogueManager.Enable();
            Inventory.Enable();
        }

        public void Disable()
        {
            PlayerManager.Disable();
            DialogueManager.Disable();
            Inventory.Disable();
        }

        public void GameOver()
        {
            Disable();
            GameManager.GameOver();
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

        public MusicManager GetSoundManager()
        {
            return MusicManager;
        }

        public Inventory GetInventory()
        {
            return Inventory;
        }

        public NPCManager GetNPCManager()
        {
            return NPCManager;
        }

        public MissionManager GetMissionManager()
        {
            return MissionManager;
        }

    }
}