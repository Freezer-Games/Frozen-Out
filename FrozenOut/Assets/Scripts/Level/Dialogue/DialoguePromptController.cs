using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class DialoguePromptController : MonoBehaviour
    {
        public abstract void Open(DialogueActer acter);
        public abstract void Close(DialogueActer acter);
    }
}