using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIController : MonoBehaviour
    {
        protected Canvas ControllerCanvas;
        public virtual bool IsOpen => ControllerCanvas.enabled;

        void Awake()
        {
            ControllerCanvas = GetComponent<Canvas>();
        }

        public virtual void Open()
        {
            ControllerCanvas.enabled = true;
        }

        public virtual void Close()
        {
            ControllerCanvas.enabled = false;
        }
    }

    public abstract class UIController<T> : UIController
    {
        
        public virtual void Open(T passingObject)
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }
    }
}