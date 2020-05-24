using System.Collections.Generic;

namespace Scripts.Level.Dialogue.Utils
{
    public interface IDialogueText
    {
        void AddText(string text);
        void AddText(IDialogueText dialogueText);

        IEnumerable<string> ParseAccumulated();
        IEnumerable<string> ParseSingle();

        string ToStringClean();
        string ToStringFull();
    }
}