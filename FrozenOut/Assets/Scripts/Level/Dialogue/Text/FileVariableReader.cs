using System;
using System.Collections.Generic;
using System.IO;

namespace Scripts.Level.Dialogue.Text
{
    public class FileVariableReader
    {
        private const string DefaultSeparator = "===";

        public static readonly FileVariableReader EqualFileVariableReader = new FileVariableReader(
            DefaultSeparator
        );

        public FileVariableReader(string nameValueSeparator)
        {
            this.NameValueSeparator = nameValueSeparator;
        }

        public string NameValueSeparator
        {
            get;
            private set;
        }

        public IDictionary<string, string> Extract(string text)
        {
            IDictionary<string, string> variables = new Dictionary<string, string>();

            using (StringReader textReader = new StringReader(text))
            {
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    int splitIndex = IndexOfNextNameValueSeparator(line);

                    string name = line.Substring(0, splitIndex);
                    string value = line.Substring(splitIndex + NameValueSeparator.Length);

                    variables[name] = value;
                }
            }

            return variables;
        }

        private int IndexOfNextNameValueSeparator(string text) => text.IndexOf(NameValueSeparator);
    }
}