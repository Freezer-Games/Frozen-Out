using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue.Runner
{
    public abstract class ChoiceSystem : MonoBehaviour
    {
        public abstract bool IsRunning();

        public abstract void StartChoice(IEnumerable<DialogueChoice> options);
    }
}