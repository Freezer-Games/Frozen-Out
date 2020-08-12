using UnityEngine;

namespace Scripts.Level.Dialogue
{
    public abstract class StylesStorage : MonoBehaviour
    {
        public abstract DialogueStyle GetStyle(string characterName);
    }
}