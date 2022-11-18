using System;
using System.Collections.Generic;
using System.Text;

namespace Ludwig.Presentation.Utilities
{
    public abstract class CsvConvert<T>
    {
        private readonly StringBuilder _content;

        public CsvConvert()
        {
            _content = new StringBuilder();
        }

        public CsvConvert<T> Add(T row)
        {
            var line = CommaSeparate(row);

            _content.Append(line).Append("\r\n");

            return this;
        }

        public CsvConvert<T> Add(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                Add(value);
            }

            return this;
        }


        public override string ToString()
        {
            var content = "";

            var headers = GetHeaders();

            if (!string.IsNullOrWhiteSpace(headers))
            {
                content = headers + "\n";
            }
            
            return content + _content.ToString();
        }

        protected string Escape(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            value = value.Trim();
            if (value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = EscapeQuoted(value, '"');
            }
            else if (value.StartsWith("'") && value.EndsWith("'"))
            {
                value = EscapeQuoted(value, '\'');
            }
            else
            {
                value = EscapeRaw(value);
            }

            return value;
        }

        private string EscapeRaw(string value)
        {
            var dq = value.IndexOf("\"", StringComparison.Ordinal) > -1;
            var sq = value.IndexOf("'", StringComparison.Ordinal) > -1;
            var comma = value.IndexOf(",", StringComparison.Ordinal) > -1;

            if (dq && sq)
            {
                value = $"\"{value}\"";

                return EscapeQuoted(value, '"');
            }
            if (dq)
            {
                return $"'{value}'";
            }
            if (sq)
            {
                return $"\"{value}\"";
            }
            if (comma)
            {
                return $"\"{value}\"";
            }
            return value;
        }

        private string EscapeQuoted(string value, char quot)
        {
            value = value.Substring(1, value.Length - 2);

            var chars = value.ToCharArray();

            var escaped = "";

            for (int i = 0; i < chars.Length; i++)
            {
                var c = chars[i];

                if (c == quot)
                {
                    escaped += quot;
                }

                escaped += c;
            }

            return quot + escaped + quot;
        }

        protected abstract string CommaSeparate(T value);

        protected virtual string GetHeaders()
        {
            return "";
        }
    }
}