using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class SelectLoadController : MonoBehaviour
    {

        public MainMenuController MainMenuController;

        public Canvas SelectLoadCanvas;
        
        public Button LoadButton;
        public Button CancelButton;
        public Scrollbar LoadsScrollbar;

        void Start()
        {
            CancelButton.onClick.AddListener(Cancel);
        }

        public void Open()
        {
            SelectLoadCanvas.enabled = true;
        }

        public void Close()
        {
            SelectLoadCanvas.enabled = false;
        }

        private void Cancel()
        {
            Close();
        }

    }
}