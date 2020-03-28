using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Localisation;

namespace Scripts.Menu.Pause
{
    public class PauseMenuManager : MonoBehaviour
    {

        public LocalisationManager LocalisationManager;
        
        public bool IsOpen
        {
            get;
            private set;
        }

        public bool IsEnabled
        {
            get;
            private set;
        }

        void Start()
        {
            IsEnabled = true;
            IsOpen = false;

            LocalisationManager.LoadLocalisedText("Menu_pausa_Default.json");
        }

        public void Open()
        {
            IsOpen = true;
        }
        public void Close()
        {
            IsOpen = false;
        }
        public void Disable()
        {
            IsEnabled = false;
        }
        public void Enable()
        {
            IsEnabled = true;
        }
        
    }

}