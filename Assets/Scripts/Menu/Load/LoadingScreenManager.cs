using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Menu.Load
{
    public class LoadingScreenManager : MonoBehaviour
    {

        public Canvas LoadCanvas;

        public Canvas IntroCanvas;

        public bool IsLoading
        {
            get;
            private set;
        }

        private GameManager GameManager => GameManager.Instance;

        void Start()
        {

        }

        public void ShowLoading()
        {
            LoadCanvas.enabled = true;
        }

        public void HideLoading()
        {
            LoadCanvas.enabled = false;
        }

        public void ShowIntro()
        {
            IntroCanvas.enabled = true;
        }

        public void HideIntro()
        {
            IntroCanvas.enabled = false;
        }

    }
}