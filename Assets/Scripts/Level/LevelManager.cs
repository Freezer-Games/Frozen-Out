using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Audio;
using Scripts.Level.Camera;
using Scripts.Level.Dialogue;
using Scripts.Level.Player;

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
        public AudioManager AudioManager
        {
            get;
            private set;
        }
        public CameraManager CameraManager
        {
            get;
            private set;
        }
        private NPCManager[] NPCs;
        private Mission[] Missions;

        public void Load()
        {
            // TODO
        }

        public PlayerManager GetPlayer()
        {
            return Player;
        }
        public IDialogueManager GetDialogueManager()
        {
            return DialogueManager;
        }
        public AudioManager GetAudioManager()
        {
            return AudioManager;
        }
        public CameraManager GetCameraManager()
        {
            return CameraManager;
        }

    }
}