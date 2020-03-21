using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {
        
        public bool IsOpen
        {
            get;
            private set;
        }

        public void Open()
        {
            IsOpen = true;
            // TODO
        }
        public void Close()
        {
            IsOpen = false;
        }
        // TODO
    }

}