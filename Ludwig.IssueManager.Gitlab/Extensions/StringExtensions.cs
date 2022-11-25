using System;
using System.Text.RegularExpressions;
using Ludwig.Contracts.Models;

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

        public static void UpdateStoryAndDescription(this string value, Issue issue)
        {
            if (issue == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                var st = value.IndexOf("$", StringComparison.Ordinal);

                if (st > -1)
                {
                    var storyWithTrailing = value.Substring(st, value.Length - st);

                    var segments = Regex.Split(storyWithTrailing, "\\s");

                    if (segments.Length > 0)
                    {
                        var story = segments[0];

                        var description = value.Replace(story, "")
                            .NormalizeWhiteSpaces().Trim();

                        issue.UserStory = story;

                        issue.Description = description;

                        return;
                    }
                }

                issue.Description = value;
            }
        }


        public static string NormalizeWhiteSpaces(this string value)
        {
            if (value == null)
            {
                return null;
            }

            value = Regex.Replace(value, "\\s+", " ");

            return value;
        }
    }
}