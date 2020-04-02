using System.Collections.Generic;
using System;
using UnityEngine;

namespace Scripts.Save
{
    public class SaveManager : MonoBehaviour
    {

        private GameManager GameManager => GameManager.Instance;

        public SaveSystem SaveSystem;
        
        //TODO
        public void Save()
        {

        }

        public void Load(int loadIndex)
        {
            
        }

        public void LoadLastLevel()
        {
            
        }

        public List<string> GetSaves()
        {
            throw new NotImplementedException();
        }

    }
}