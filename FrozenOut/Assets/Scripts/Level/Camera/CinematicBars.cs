using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Camera
{
    public class CinematicBars : MonoBehaviour
    {
        
        private RectTransform TopBar, BottomBar;
        private float ChangeSizeAmount;
        private float TargetSize;
        private bool IsActive;

        void Awake() 
        {
            GameObject gameObject = new GameObject("topBar", typeof(Image));
            gameObject.transform.SetParent(transform, false);
            gameObject.GetComponent<Image>().color = Color.black;
            TopBar = gameObject.GetComponent<RectTransform>();
            TopBar.anchorMin = new Vector2(0, 1);
            TopBar.anchorMax = new Vector2(1, 1);
            TopBar.sizeDelta = new Vector2(0, 0);

            gameObject = new GameObject("bottomBar", typeof(Image));
            gameObject.transform.SetParent(transform, false);
            gameObject.GetComponent<Image>().color = Color.black;
            BottomBar = gameObject.GetComponent<RectTransform>();
            BottomBar.anchorMin = new Vector2(0, 0);
            BottomBar.anchorMax = new Vector2(1, 0);
            BottomBar.sizeDelta = new Vector2(0, 0);
        }

        private void Update() 
        {
            if (IsActive) {
                Vector2 sizeDelta = TopBar.sizeDelta;
                sizeDelta.y += ChangeSizeAmount * Time.deltaTime;

                if (ChangeSizeAmount > 0) {
                    if (sizeDelta.y >= TargetSize) {
                        sizeDelta.y = TargetSize;
                        IsActive = false;
                    }
                } else {
                    if (sizeDelta.y <= TargetSize) {
                        sizeDelta.y = TargetSize;
                        IsActive = false;
                    }
                }
                TopBar.sizeDelta = sizeDelta;
                BottomBar.sizeDelta = sizeDelta;
            }
        }

        public void Show(float targetSize, float time) 
        {
            TargetSize = targetSize;
            ChangeSizeAmount = (targetSize - TopBar.sizeDelta.y) / time;
            IsActive = true;
        }

        public void Hide(float time) 
        {
            TargetSize = 0;
            ChangeSizeAmount = (TargetSize - TopBar.sizeDelta.y) / time;
            IsActive = true;
        }

    }
}