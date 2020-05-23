using System;

namespace Scripts.Level.Dialogue.Utils.Tag
{
    /// <summary>
    /// Representa opción de un tag (size=20, color)
    /// </summary>
    public class TagOption
    {
        private const char EQUAL_SIGN = '=';

        public TagOption(string option, TagFormat format, TagOptionPosition position = TagOptionPosition.Start)
        {
            this.Option = option;
            this.Format = format;
            this.Position = position;
        }

        public string Option
        {
            get;
            private set;
        }
        public TagFormat Format
        {
            get;
            private set;
        }
        public TagOptionPosition Position
        {
            get;
            private set;
        }

        public override string ToString() => Text();

        /// Obtiene el tag completo (ej.: <size=20>)
        public string Text()
        {
            switch (Position)
            {
                case TagOptionPosition.Start:
                    return $"{Format.StartSeparator}{Option}{Format.EndSeparator}";
                case TagOptionPosition.End:
                    return $"{Format.StartSeparator}{Format.EndOptionSeparator}{Option}{Format.EndSeparator}";
                default:
                    return null;
            };
        }

        public string MainOption()
        {
            string mainOption = null;
            if(Option != null)
            {
                mainOption = Option;

                //Si existe un signo de igual
                if (Option.IndexOf(EQUAL_SIGN) >= 0)
                {
                    mainOption = Option.Split(EQUAL_SIGN)[0];
                }
            }

            return mainOption;
        }

        public static bool Matches(TagOption start, TagOption end)
        {
            return start.Position == TagOptionPosition.Start
                && end.Position == TagOptionPosition.End
                && start.MainOption().Equals(end.MainOption());
        }
    }

    public enum TagOptionPosition
    {
        Start,
        End
    }
}