using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Settings;

using Scripts.Level.Sound;
using Scripts.Level.Camera;
using Scripts.Level.Player;
using Scripts.Level.Dialogue;
using Scripts.Level.Dialogue.YarnSpinner;
using Scripts.Level.Mission;
using Scripts.Level.NPC;
using Scripts.Level.Item;

namespace Scripts.Level
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {

        public PlayerManager PlayerManager;
        //En cualquier otro lugar debería usarse IDialogueManager
        //Unity no permite interfaces en el inspector, hay que usar una clase concreta aquí
        public YarnManager DialogueManager;
        public SoundManager SoundManager;
        public CameraManager CameraManager;
        public Inventory Inventory;
        public NPCInfo[] NPCs;
        public MissionInfo[] Missions;

        private GameManager GameManager => GameManager.Instance;
        private SettingsManager SettingsManager => GameManager.SettingsManager;

        void Start()
        {

        }

        public void Load()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            AudioListener.volume = Mathf.Clamp(SettingsManager.MusicVolume / 100f, 0, 1);
            
            if(DialogueManager != null)
            {
                DialogueManager.Started += (sender, args) => SoundManager.DecreaseVolume();
                DialogueManager.Ended += (sender, args) => SoundManager.IncreaseVolume();
                DialogueManager.Started += (sender, args) => PlayerManager.Disable();
                DialogueManager.Ended += (sender, args) => PlayerManager.Enable();
            }

            if(Inventory != null)
            {
                Inventory.CloseMenu();
                Inventory.CloseUsePrompt();
            }

            if(PlayerManager != null)
            {
                PlayerManager.Enable();
            }
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

        public IDialogueManager GetDialogueManager()
        {
            return DialogueManager;
        }

        public SoundManager GetSoundManager()
        {
            return SoundManager;
        }
        
        public CameraManager GetCameraManager()
        {
            return CameraManager;
        }

        public Inventory GetInventory()
        {
            return Inventory;
        }

        public NPCInfo[] GetNPCs()
        {
            return NPCs;
        }

        public MissionInfo[] GetMissions()
        {
            return Missions;
        }

    }
}