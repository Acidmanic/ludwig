using System;
using System.Text.RegularExpressions;

namespace Ludwig.IssueManager.Gitlab.Extensions
{
    public static class StringExtensions
    {


        public static string ExtractStory(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var st = value.IndexOf("$", StringComparison.Ordinal);

                if (st > -1)
                {
                    var storyWithTrailing = value.Substring(st, value.Length - st);

                    var segments = Regex.Split(storyWithTrailing, "\\s");

                    if (segments.Length > 0)
                    {
                        return segments[0];
                    }
                }
            }

            return "Story";
        }
    }
}