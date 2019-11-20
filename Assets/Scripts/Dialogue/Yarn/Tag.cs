namespace Assets.Scripts.Dialogue.Yarn
{
    public class Tag
    {
        public const char SEPARATOR_INIT = '<';
        public const char SEPARATOR_END = '>';
        public const char OPTION_END = '/';

        public string Option { get; set; }

        public TagOption StartOption => new TagOption(Option, TagOptionPosition.start);
        public TagOption EndOption => new TagOption(Option, TagOptionPosition.end);

        public Tag(string option)
        {
            this.Option = option;
        }

        public string GetTaggedText(string text) => StartOption.Text + text + EndOption.Text;
    }
}
