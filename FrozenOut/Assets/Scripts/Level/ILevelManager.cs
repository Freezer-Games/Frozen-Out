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
        void Enable();
        void Disable();

        void GameOver();

        SettingsManager GetSettingsManager();

        PlayerManager GetPlayerManager();
        DialogueManager GetDialogueManager();
        MusicManager GetSoundManager();
        //CameraManager GetCameraManager();
        Inventory GetInventory();
        NPCManager GetNPCManager();
        MissionManager GetMissionManager();
    }
}