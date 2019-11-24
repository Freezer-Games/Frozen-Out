namespace Assets.Scripts.Dialogue.Texts.Tags
{
    public class TagOption
    {
        public const char EQUAL_SIGN = '=';

        public string Option { get; }

        public string MainOption =>
            (Option == null)
            ? null
            : (Option.IndexOf(EQUAL_SIGN) < 0)
                ? Option
                : Option.Split(EQUAL_SIGN)[0];

        public TagFormat Format { get; }

        public TagOptionPosition Position { get; }

        public string Text
        {
            get
            {
                // No se puede convertir en expresión switch, porque Unity no lo entiende
                // (es una adición a CSharp muy nueva)
                switch (Position)
                {
                    case TagOptionPosition.start: return $"{Format.StartSeparator}{Option}{Format.EndSeparator}";
                    case TagOptionPosition.end: return $"{Format.StartSeparator}{Format.EndOptionSeparator}{Option}{Format.EndSeparator}";
                    default: return null;
                };
            }
        }

        public TagOption(string option, TagFormat format, TagOptionPosition position = TagOptionPosition.start)
        {
            this.Option = option;
            this.Format = format;
            this.Position = position;
        }

        public override string ToString() => this.Text;

        public static bool Matches(TagOption start, TagOption end)
        {
            return start.Position == TagOptionPosition.start
                && end.Position == TagOptionPosition.end
                && start.MainOption == end.MainOption;
        }
    }
}
