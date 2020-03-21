using System.Collections;
using System.Collections.Generic;

using Scripts.Level.Sound;
using Scripts.Level.Dialogue;
using Scripts.Level.Player;
using Scripts.Level.Camera;

namespace Scripts.Level
{
    public interface ILevelManager
    {

        void Load();
        void Unload();

        PlayerManager GetPlayerManager();
        IDialogueManager GetDialogueManager();
        SoundManager GetSoundManager();
        CameraManager GetCameraManager();
        // TODO
    }
}