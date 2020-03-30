using System.Collections;
using System.Collections.Generic;

using Scripts.Settings;

using Scripts.Level.Sound;
using Scripts.Level.Player;
using Scripts.Level.Dialogue;
using Scripts.Level.Camera;
using Scripts.Level.Item;
using Scripts.Level.NPC;
using Scripts.Level.Mission;


namespace Scripts.Level
{
    public interface ILevelManager
    {

        void Load();
        void Unload();

        SettingsManager GetSettingsManager();

        PlayerManager GetPlayerManager();
        IDialogueManager GetDialogueManager();
        SoundManager GetSoundManager();
        CameraManager GetCameraManager();
        Inventory GetInventory();
        NPCInfo[] GetNPCs();
        MissionInfo[] GetMissions();
        // TODO
    }
}