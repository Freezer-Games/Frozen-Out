using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Sound;
using Scripts.Level.Camera;
using Scripts.Level.Dialogue;
using Scripts.Level.Player;
using Scripts.Level.Mission;
using Scripts.Level.NPC;

namespace Scripts.Level
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {

        public PlayerManager Player
        {
            get;
            private set;
        }
        public IDialogueManager DialogueManager
        {
            get;
            private set;
        }
        public SoundManager SoundManager
        {
            get;
            private set;
        }
        public CameraManager CameraManager
        {
            get;
            private set;
        }
        private NPCInfo[] NPCs;
        private MissionInfo[] Missions;

        void Start()
        {
            DialogueManager.Started += (sender, args) => SoundManager.DecreaseVolume();
            DialogueManager.Ended += (sender, args) => SoundManager.IncreaseVolume();
        }

        public void Load()
        {
            // TODO
        }
        public void Unload()
        {
            // TODO
        }

        public PlayerManager GetPlayerManager()
        {
            return Player;
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

    }
}