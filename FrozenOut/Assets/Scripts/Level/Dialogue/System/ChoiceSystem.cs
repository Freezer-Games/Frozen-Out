using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue.System
{
    public abstract class ChoiceSystem : MonoBehaviour
    {
        public abstract void StartChoice(IEnumerable<DialogueChoice> choices);
    }
}