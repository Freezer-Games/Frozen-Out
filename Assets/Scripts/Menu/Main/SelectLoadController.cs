using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class SelectLoadController : UIController
    {

        public UIController MainMenuController;
        
        public Button LoadButton;
        public Button CancelButton;
        public Scrollbar LoadsScrollbar;

        void Start()
        {
            CancelButton.onClick.AddListener(Cancel);
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        private void Cancel()
        {
            Close();
        }

    }
}