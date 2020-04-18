using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Text
{
    public interface IDialogueText
    {
        IEnumerable<string> Parse();

        void AddText(string text);
        void AddText(IDialogueText dialogueText);
    }
}