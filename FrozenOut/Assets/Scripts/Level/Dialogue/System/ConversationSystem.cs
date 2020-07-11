using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Dialogue.Runner
{
    public abstract class ConversationSystem : MonoBehaviour
    {
        public abstract bool IsRunning();

        public abstract void StartOptions(IEnumerable<DialogueOption> options);
    }
}