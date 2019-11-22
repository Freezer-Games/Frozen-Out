namespace Assets.Scripts.Dialogue.Texts.Tags
{
    /// <summary>
    /// Tags disponibles en Unity: https://docs.unity3d.com/Manual/StyledText.html
    /// </summary>
    public class Tag
    {
        public string Option { get; }

        public TagFormat Format { get; set; }

        public TagOption StartOption { get; }
        public TagOption EndOption { get; }

        public Tag(string option, TagFormat format)
        {
            this.Option = option;
            this.StartOption = new TagOption(option, format, TagOptionPosition.start);
            this.EndOption = new TagOption(option, format, TagOptionPosition.end);
        }

        public Tag(TagOption startOption, TagOption endOption)
        {
            this.Option = startOption.MainOption;
            this.Format = startOption.Format;

            this.StartOption = startOption;
            this.EndOption = endOption;
        }

        public string GetTaggedText(string text) => StartOption.Text + text + EndOption.Text;
    }
}
