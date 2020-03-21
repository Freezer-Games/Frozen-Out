using System.Collections;
using System.Collections.Generic;

using Scripts.Level.Audio;
using Scripts.Level.Dialogue;
using Scripts.Level.Player;
using Scripts.Level.Camera;

namespace Scripts.Level
{
    public interface ILevelManager
    {

        void Load();

        PlayerManager GetPlayer();
        IDialogueManager GetDialogueManager();
        AudioManager GetAudioManager();
        CameraManager GetCameraManager();
        // TODO
    }
}