using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Level.Dialogue;

namespace Scripts.Level.Camera
{
    public class CameraManager : MonoBehaviour
    {
        
        public LevelManager LevelManager;
        
        public UnityEngine.Camera MainCamera;

        public GameObject GetPlayerObject()
        {
            return LevelManager.GetPlayerManager().Player;
        }

        public UnityEngine.Camera GetMainCamera()
        {
            return MainCamera;
        }

    }
}