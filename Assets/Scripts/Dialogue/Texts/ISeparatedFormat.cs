namespace Assets.Scripts.Dialogue.Texts
{
    public interface ISeparatedFormat<T>
    {
        string EndSeparator { get; }
        string StartSeparator { get; }

        T Extract(string line, out int startingIndex, out int endIndex, out string remainingText);
        bool HasAnyTags(string text);
        int IndexOfNextEnd(string text);
        int IndexOfNextStart(string text);
    }
}