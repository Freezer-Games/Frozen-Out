using System.Collections;
using System.Collections.Generic;

using Scripts.Level.Audio;
using Scripts.Level.Dialogue;
using Scripts.Level.Player;

namespace Scripts.Level
{
    public interface ILevelManager
    {

        public void Load();

        public PlayerManager GetPlayer();
        public IDialogueManager GetDialogueManager();
        public AudioManager GetAudioManager();
        public CameraManager GetCameraManager();
        // TODO
    }
}