using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public abstract class TriggerBase : MonoBehaviour
    {
        protected GameManager GameManager => GameManager.Instance;

        protected const string PlayerTag = "Player";
    }
}