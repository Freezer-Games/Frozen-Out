namespace Assets.Scripts.Dialogue.Texts.Tags
{
    /// <summary>
    /// Tags disponibles en Unity: https://docs.unity3d.com/Manual/StyledText.html
    /// </summary>
    public class Tag
    {
        public const char SEPARATOR_INIT = '<';
        public const char SEPARATOR_END = '>';
        public const char OPTION_END = '/';

        public string Option { get; }

        public TagOption StartOption { get; }
        public TagOption EndOption { get; }

        public Tag(string option)
        {
            this.Option = option;
            this.StartOption = new TagOption(option, TagOptionPosition.start);
            this.EndOption = new TagOption(option, TagOptionPosition.end);
        }

        public Tag(TagOption startOption, TagOption endOption)
        {
            this.Option = startOption.MainOption;
            this.StartOption = startOption;
            this.EndOption = endOption;
        }

        public string GetTaggedText(string text) => StartOption.Text + text + EndOption.Text;


        public static int IndexOfNextTagInit(string text) => text.IndexOf(SEPARATOR_INIT);
        public static int IndexOfNextOptionEnd(string text) => text.IndexOf(OPTION_END);
        public static int IndexOfNextTagEnd(string text) => text.IndexOf(SEPARATOR_END);

        public static bool HasAnyTags(string text) => IndexOfNextTagInit(text) >= 0;
    }
}
