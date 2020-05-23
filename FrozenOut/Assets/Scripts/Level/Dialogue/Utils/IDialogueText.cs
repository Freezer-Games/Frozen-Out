using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Utils
{
    public interface IDialogueText
    {
        IEnumerable<string> Parse();

        void AddText(string text);
        void AddText(IDialogueText dialogueText);
    }
}