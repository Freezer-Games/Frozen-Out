using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu.Main
{
    public class OptionsMenuController : UIController
    {
        public MainMenuManager MainMenuManager;

        public UIController GameSectionController;
        public UIController AudioSectionController;
        public UIController GraphicsSectionController;
        public UIController ControlsSectionController;

        public Button ConfirmButton;
        public Button CancelButton;

        public Button GameSectionButton;
        public Button AudioSectionButton;
        public Button GraphicsSectionButton;
        public Button ControlsSectionButton;
        
        void Start()
        {
            ConfirmButton.onClick.AddListener(Confirm);
            CancelButton.onClick.AddListener(OpenMainMenu);

            GameSectionButton.onClick.AddListener(OpenGameSection);
            AudioSectionButton.onClick.AddListener(OpenAudioSection);
            GraphicsSectionButton.onClick.AddListener(OpenGraphicsSection);
            ControlsSectionButton.onClick.AddListener(OpenControlsSection);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                OpenMainMenu();
            }
        }

        public override void Open()
        {
            base.Open();

            OpenGameSection();
        }

        public override void Close()
        {
            base.Close();
        }

        private void OpenGameSection()
        {
            GameSectionController.Open();
            AudioSectionController.Close();
            GraphicsSectionController.Close();
            ControlsSectionController.Close();
        }

        private void OpenAudioSection()
        {
            GameSectionController.Close();
            AudioSectionController.Open();
            GraphicsSectionController.Close();
            ControlsSectionController.Close();
        }

        private void OpenGraphicsSection()
        {
            GameSectionController.Close();
            AudioSectionController.Close();
            GraphicsSectionController.Open();
            ControlsSectionController.Close();
        }

        private void OpenControlsSection()
        {
            GameSectionController.Close();
            AudioSectionController.Close();
            GraphicsSectionController.Close();
            ControlsSectionController.Open();
        }

        private void OpenMainMenu()
        {
            Close();
            MainMenuManager.OpenMainMenu();
        }
        
        void Confirm()
        {
            OpenMainMenu();
        }

    }
}