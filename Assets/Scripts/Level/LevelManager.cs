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

        private PlayerManager Player
        {
            private set;
        }
        public IDialogueManager DialogueManager
        {
            private set;
        }
        public AudioManager AudioManager
        {
            private set;
        }
        public CameraManager CameraManager
        {
            private set;
        }
        private NPC[] NPCs;
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