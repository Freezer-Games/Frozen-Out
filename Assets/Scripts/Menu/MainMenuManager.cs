using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Localisation;

namespace Scripts.Menu.Main
{
    public class MainMenuManager : MonoBehaviour
    {

        public LocalisationManager LocalisationManager;

        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            LocalisationManager.LoadLocalisedText("Menu_Default.json");
        }
        
    }
}