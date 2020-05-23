using UnityEngine;

namespace Scripts.Level.Dialogue.Text
{
    public abstract class TextManager : MonoBehaviour
    {
        public abstract void Open();
        public abstract void Close();

        public abstract void SetStyle(TextStyle style);

        public abstract void ShowName(string name);
        public abstract void ShowName(char newNameLetter);
        public abstract void ShowDialogue(string dialogue);
        public abstract void ShowDialogue(char newDialogueLetter);
    }
}