using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Dialogue.Texts
{
    public interface IDialogueText
    {
        IEnumerable<char> Parse();
        IEnumerable<string> ParseInBuilder(StringBuilder builder);

        void AddText(string text);
        void AddDialogueText(IDialogueText dialogueText);
    }
}