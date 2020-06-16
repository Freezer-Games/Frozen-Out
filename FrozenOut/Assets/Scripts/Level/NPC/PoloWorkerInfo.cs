using System.Collections;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class PoloWorkerInfo : PoloInfo
    {
        void Start()
        {
            StartAnimation("Work");
        }
    }
}