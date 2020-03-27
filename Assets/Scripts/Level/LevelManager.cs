using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public PlayerManager Player;
        //En cualquier otro lugar debería usarse IDialogueManager
        //Unity no permite interfaces en el inspector, hay que usar una clase concreta aquí
        public YarnManager DialogueManager;
        public SoundManager SoundManager;
        public CameraManager CameraManager;
        public Inventory Inventory;
        public NPCInfo[] NPCs;
        public MissionInfo[] Missions;

        void Start()
        {
            
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